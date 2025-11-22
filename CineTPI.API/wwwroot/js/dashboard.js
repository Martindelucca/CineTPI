document.addEventListener('DOMContentLoaded', function () {

    const token = localStorage.getItem('token');
    if (!token) {
        alert("Sesión expirada. Por favor, vuelva a iniciar sesión.");
        window.location.href = "login.html";
        return;
    }

    //  RESERVAS POR ESTADO
    const tbodyReservas = document.getElementById('tbody-reporte-reservas');

    async function cargarReporteReservas() {
        tbodyReservas.innerHTML = '<tr><td colspan="3">Cargando...</td></tr>';

        try {
            const response = await fetch('/api/dashboard/reservas-por-estado', {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (!response.ok) throw new Error(`Error HTTP ${response.status}`);

            const data = await response.json();
            renderReporteReservas(data);
            renderGraficoReservas(data); 
        } catch (error) {
            console.error('Error Reporte 1:', error);
            tbodyReservas.innerHTML = '<tr><td colspan="3">Error al cargar reporte.</td></tr>';
        }
    }

    function renderReporteReservas(data) {
        tbodyReservas.innerHTML = '';
        if (!data || data.length === 0) {
            tbodyReservas.innerHTML = '<tr><td colspan="3">No se encontraron reservas.</td></tr>';
            return;
        }

        data.forEach(item => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${item.estadoReserva}</td>
                <td>${item.cantidadReservas}</td>
                <td>$ ${Number(item.totalRecaudado).toFixed(2)}</td>
            `;
            tbodyReservas.appendChild(tr);
        });
    }

    // REPORTE 2: RECAUDACIÓN POR PELÍCULA
    const formRecaudacion = document.getElementById('form-recaudacion');
    const tbodyRecaudacion = document.getElementById('tbody-reporte-recaudacion');
    const inputFechaDesde = document.getElementById('fecha-desde');
    const inputFechaHasta = document.getElementById('fecha-hasta');

    const hoy = new Date();
    const primerDiaMes = new Date(hoy.getFullYear(), hoy.getMonth(), 1).toISOString().split('T')[0];
    const hoyISO = hoy.toISOString().split('T')[0];
    inputFechaDesde.value = primerDiaMes;
    inputFechaHasta.value = hoyISO;

    formRecaudacion.addEventListener('submit', function (e) {
        e.preventDefault();
        cargarReporteRecaudacion();
    });

    async function cargarReporteRecaudacion() {
        const fechaDesde = inputFechaDesde.value;
        const fechaHasta = inputFechaHasta.value;

        if (!fechaDesde || !fechaHasta) {
            alert('Seleccione ambas fechas.');
            return;
        }

        tbodyRecaudacion.innerHTML = '<tr><td colspan="4">Cargando...</td></tr>';

        try {
            const response = await fetch(
                `/api/dashboard/recaudacion-por-pelicula?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`,
                { headers: { 'Authorization': `Bearer ${token}` } }
            );

            if (!response.ok) throw new Error(`Error HTTP ${response.status}`);

            const data = await response.json();
            renderReporteRecaudacion(data);
            renderGraficoRecaudacion(data); // agregado
        } catch (error) {
            console.error('Error Reporte 2:', error);
            tbodyRecaudacion.innerHTML = '<tr><td colspan="4">Error al cargar reporte.</td></tr>';
        }
    }

    function renderReporteRecaudacion(data) {
        tbodyRecaudacion.innerHTML = '';
        if (!data || data.length === 0) {
            tbodyRecaudacion.innerHTML = '<tr><td colspan="4">Sin resultados para ese rango.</td></tr>';
            return;
        }

        data.forEach(item => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${item.idPelicula}</td>
                <td>${item.pelicula}</td>
                <td>${item.totalEntradasVendidas}</td>
                <td>$ ${Number(item.totalRecaudado).toFixed(2)}</td>
            `;
            tbodyRecaudacion.appendChild(tr);
        });
    }

    // REPORTE 3: CLIENTES FRECUENTES
    const tbodyClientes = document.getElementById('tbody-reporte-clientes');

    async function cargarReporteClientes() {
        tbodyClientes.innerHTML = '<tr><td colspan="5">Cargando...</td></tr>';

        try {
            const response = await fetch('/api/dashboard/clientes-frecuentes', {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (!response.ok) throw new Error(`Error HTTP ${response.status}`);

            const data = await response.json();
            renderReporteClientes(data);
            renderGraficoTopClientes(data); // agregado
        } catch (error) {
            console.error('Error Reporte 3:', error);
            tbodyClientes.innerHTML = '<tr><td colspan="5">Error al cargar reporte.</td></tr>';
        }
    }

    function renderReporteClientes(data) {
        tbodyClientes.innerHTML = '';
        if (!data || data.length === 0) {
            tbodyClientes.innerHTML = '<tr><td colspan="5">Sin clientes frecuentes.</td></tr>';
            return;
        }

        data.forEach(item => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${item.codCliente}</td>
                <td>${item.nombreCliente} ${item.apellidoCliente}</td>
                <td>${item.peliculasDistintas}</td>
                <td>${item.totalEntradas}</td>
                <td>$ ${Number(item.totalGastado).toFixed(2)}</td>
            `;
            tbodyClientes.appendChild(tr);
        });
    }

    // REPORTE 4: FUNCIONES POR GÉNERO
    const formFuncionesGenero = document.getElementById('form-funciones-genero');
    const tbodyFuncionesGenero = document.getElementById('tbody-funciones-genero');

    formFuncionesGenero.addEventListener('submit', async (e) => {
        e.preventDefault();

        const genero = document.getElementById('genero').value;
        const fechaDesde = document.getElementById('fecha-desde-gen').value;
        const fechaHasta = document.getElementById('fecha-hasta-gen').value;

        if (!fechaDesde || !fechaHasta) {
            alert('Debe seleccionar un rango de fechas.');
            return;
        }

        tbodyFuncionesGenero.innerHTML = '<tr><td colspan="8">Cargando...</td></tr>';

        try {
            const url = `/api/dashboard/funciones-por-genero?genero=${encodeURIComponent(genero)}&fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`;
            const response = await fetch(url, { headers: { 'Authorization': `Bearer ${token}` } });

            if (!response.ok) throw new Error('Error al cargar las funciones.');

            const data = await response.json();
            renderFuncionesGenero(data);
        } catch (error) {
            console.error('Error Reporte 4:', error);
            tbodyFuncionesGenero.innerHTML = '<tr><td colspan="8">Error al cargar funciones.</td></tr>';
        }
    });

    function renderFuncionesGenero(data) {
        tbodyFuncionesGenero.innerHTML = '';
        if (!data || data.length === 0) {
            tbodyFuncionesGenero.innerHTML = '<tr><td colspan="8">Sin resultados</td></tr>';
            return;
        }

        data.forEach(f => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${f.titulo}</td>
                <td>${f.genero}</td>
                <td>${f.sala}</td>
                <td>${f.tipoSala}</td>
                <td>${f.formato}</td>
                <td>${f.fecha ? f.fecha.split('T')[0] : ''}</td>
                <td>${f.horario}</td>
                <td>${f.director}</td>
            `;
            tbodyFuncionesGenero.appendChild(tr);
        });
    }

    // REPORTE 5: PERFIL DEL CLIENTE

    const formPerfilCliente = document.getElementById('form-perfil-cliente');
    const pcInfo = document.getElementById('pc-info');
    const pcNombre = document.getElementById('pc-nombre');
    const pcFechaRegistro = document.getElementById('pc-fecha-registro');
    const pcGastoEntradas = document.getElementById('pc-gasto-entradas');
    const pcGastoProductos = document.getElementById('pc-gasto-productos');
    const pcGeneroFav = document.getElementById('pc-genero-favorito');
    const pcTbody = document.getElementById('pc-tbody-historial');

    formPerfilCliente.addEventListener('submit', async (e) => {
        e.preventDefault();
        const clienteId = parseInt(document.getElementById('pc-cliente-id').value);
        if (!clienteId) return alert("Ingrese un ID válido");

        pcTbody.innerHTML = '<tr><td colspan="3">Cargando...</td></tr>';
        pcInfo.style.display = 'none';

        try {
            const response = await fetch(`/api/dashboard/perfil-cliente?clienteId=${clienteId}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (!response.ok) throw new Error(`Error HTTP ${response.status}`);

            const data = await response.json();
            if (!data || data.length === 0) {
                pcTbody.innerHTML = '<tr><td colspan="3">Sin información.</td></tr>';
                return;
            }

            renderPerfilCliente(data[0]);
        } catch (error) {
            console.error('Error Reporte 5:', error);
            pcTbody.innerHTML = '<tr><td colspan="3">Error al cargar perfil.</td></tr>';
        }
    });

    function renderPerfilCliente(perfil) {
        const i = perfil.info;
        pcNombre.textContent = `${i.nombre} ${i.apellido}`;
        pcFechaRegistro.textContent = i.fechaRegistro ? i.fechaRegistro.split('T')[0] : '-';
        pcGastoEntradas.textContent = `$ ${Number(i.gastoTotalEntradas).toFixed(2)}`;
        pcGastoProductos.textContent = `$ ${Number(i.gastoTotalProductos || 0).toFixed(2)}`;
        pcGeneroFav.textContent = i.generoFavorito || '—';
        pcInfo.style.display = 'flex';

        const rows = perfil.peliculasVistas || [];
        pcTbody.innerHTML = rows.length === 0
            ? '<tr><td colspan="3">Sin películas vistas.</td></tr>'
            : rows.map(r => `
                <tr>
                    <td>${r.peliculaVista}</td>
                    <td>${r.estreno ? r.estreno.split('T')[0] : ''}</td>
                    <td>${r.fechaDeLaFuncion ? r.fechaDeLaFuncion.split('T')[0] : ''}</td>
                </tr>`).join('');
    }
    //GRAFICOS
    function renderGraficoReservas(data) {
        const ctx = document.getElementById('chart-reservas');
        if (!ctx) return;
        if (ctx.chart) ctx.chart.destroy();

        ctx.chart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: data.map(x => x.estadoReserva),
                datasets: [{ data: data.map(x => x.cantidadReservas), backgroundColor: ['#198754', '#0d6efd', '#ffc107', '#dc3545'] }]
            },
            options: { plugins: { legend: { position: 'bottom' } } }
        });
    }

    function renderGraficoRecaudacion(data) {
        const ctx = document.getElementById('chart-recaudacion');
        if (!ctx) return;
        if (ctx.chart) ctx.chart.destroy();

        ctx.chart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: data.map(x => x.pelicula),
                datasets: [{ label: 'Recaudación ($)', data: data.map(x => x.totalRecaudado), backgroundColor: '#0d6efd' }]
            },
            options: { plugins: { legend: { display: false } }, scales: { y: { beginAtZero: true } } }
        });
    }

    function renderGraficoTopClientes(data) {
        const ctx = document.getElementById('chart-clientes');
        if (!ctx) return;
        if (ctx.chart) ctx.chart.destroy();

        const top = data.slice(0, 5);
        ctx.chart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: top.map(x => `${x.nombreCliente} ${x.apellidoCliente}`),
                datasets: [{ data: top.map(x => x.totalGastado), backgroundColor: '#ffcc00' }]
            },
            options: {
                indexAxis: 'y',
                plugins: { legend: { display: false } },
                scales: { x: { beginAtZero: true } }
            }
        });
    }

    // CARGA INICIAL
    cargarReporteReservas();
    cargarReporteRecaudacion();
    cargarReporteClientes();

});
