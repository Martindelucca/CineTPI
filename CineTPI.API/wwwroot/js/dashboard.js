// Esperamos a que todo el HTML (dashboard.html) esté cargado
document.addEventListener('DOMContentLoaded', function () {

    // --- CONFIGURACIÓN GLOBAL ---
    const token = localStorage.getItem('token');

    // =============================
    // REPORTE 1: RESERVAS POR ESTADO
    // =============================
    const tbodyReservas = document.getElementById('tbody-reporte-reservas');

    async function cargarReporteReservas() {
        try {
            const response = await fetch('/api/dashboard/reservas-por-estado', {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (response.ok) {
                const data = await response.json();
                renderReporteReservas(data);
            } else if (response.status === 401 || response.status === 403) {
                alert('Acceso denegado. Solo administradores pueden ver esta página.');
                window.location.href = 'index.html';
            } else {
                throw new Error('Error del servidor');
            }
        } catch (error) {
            console.error('Error Reporte 1:', error);
            tbodyReservas.innerHTML = '<tr><td colspan="3">Error al cargar reporte.</td></tr>';
        }
    }

    function renderReporteReservas(data) {
        tbodyReservas.innerHTML = '';

        if (data.length === 0) {
            tbodyReservas.innerHTML = '<tr><td colspan="3">No se encontraron reservas.</td></tr>';
            return;
        }

        data.forEach(item => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${item.estadoReserva}</td>
                <td>${item.cantidadReservas}</td>
                <td>$ ${item.totalRecaudado.toFixed(2)}</td>
            `;
            tbodyReservas.appendChild(tr);
        });
    }

    // =============================
    // REPORTE 2: RECAUDACIÓN POR PELÍCULA
    // =============================
    const formRecaudacion = document.getElementById('form-recaudacion');
    const tbodyRecaudacion = document.getElementById('tbody-reporte-recaudacion');
    const inputFechaDesde = document.getElementById('fecha-desde');
    const inputFechaHasta = document.getElementById('fecha-hasta');

    // Seteamos fechas por defecto (mes actual)
    const hoy = new Date();
    const primerDiaMes = new Date(hoy.getFullYear(), hoy.getMonth(), 1).toISOString().split('T')[0];
    const hoyISO = hoy.toISOString().split('T')[0];
    inputFechaDesde.value = primerDiaMes;
    inputFechaHasta.value = hoyISO;

    // Listener para el botón "Consultar"
    formRecaudacion.addEventListener('submit', async function (e) {
        e.preventDefault();
        cargarReporteRecaudacion();
    });

    async function cargarReporteRecaudacion() {
        const fechaDesde = inputFechaDesde.value;
        const fechaHasta = inputFechaHasta.value;

        if (!fechaDesde || !fechaHasta) {
            alert('Por favor, seleccione ambas fechas.');
            return;
        }

        tbodyRecaudacion.innerHTML = '<tr><td colspan="4">Cargando...</td></tr>';

        try {
            const response = await fetch(
                `/api/dashboard/recaudacion-por-pelicula?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`,
                {
                    headers: { 'Authorization': `Bearer ${token}` }
                }
            );

            if (response.ok) {
                const data = await response.json();
                renderReporteRecaudacion(data);
            } else if (response.status === 401 || response.status === 403) {
                alert('Acceso denegado. Vuelva a iniciar sesión.');
                window.location.href = 'login.html';
            } else {
                throw new Error('Error del servidor');
            }
        } catch (error) {
            console.error('Error Reporte 2:', error);
            tbodyRecaudacion.innerHTML = '<tr><td colspan="4">Error al cargar reporte.</td></tr>';
        }
    }

    function renderReporteRecaudacion(data) {
        tbodyRecaudacion.innerHTML = '';

        if (data.length === 0) {
            tbodyRecaudacion.innerHTML = '<tr><td colspan="4">No se encontraron resultados para esas fechas.</td></tr>';
            return;
        }

        data.forEach(item => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${item.idPelicula}</td>
                <td>${item.pelicula}</td>
                <td>${item.totalEntradasVendidas}</td>
                <td>$ ${item.totalRecaudado.toFixed(2)}</td>
            `;
            tbodyRecaudacion.appendChild(tr);
        });
    }

    // =============================
    // REPORTE 3: CLIENTES FRECUENTES
    // =============================
    const tbodyClientes = document.getElementById('tbody-reporte-clientes');

    async function cargarReporteClientes() {
        tbodyClientes.innerHTML = '<tr><td colspan="5">Cargando...</td></tr>';

        try {
            const response = await fetch('/api/dashboard/clientes-frecuentes', {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (response.ok) {
                const data = await response.json();
                renderReporteClientes(data);
            } else if (response.status === 401 || response.status === 403) {
                alert('Acceso denegado. Vuelva a iniciar sesión.');
                window.location.href = 'login.html';
            } else {
                throw new Error('Error del servidor');
            }
        } catch (error) {
            console.error('Error Reporte 3:', error);
            tbodyClientes.innerHTML = '<tr><td colspan="5">Error al cargar reporte.</td></tr>';
        }
    }

    function renderReporteClientes(data) {
        tbodyClientes.innerHTML = '';

        if (data.length === 0) {
            tbodyClientes.innerHTML = '<tr><td colspan="5">No se encontraron clientes con compras.</td></tr>';
            return;
        }

        data.forEach(item => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${item.codCliente}</td>
                <td>${item.nombreCliente} ${item.apellidoCliente}</td>
                <td>${item.peliculasDistintas}</td>
                <td>${item.totalEntradas}</td>
                <td>$ ${item.totalGastado.toFixed(2)}</td>
            `;
            tbodyClientes.appendChild(tr);
        });
    }

    // =============================
    // CARGA INICIAL DE REPORTES
    // =============================
    cargarReporteReservas();
    cargarReporteRecaudacion();
    cargarReporteClientes();

});
