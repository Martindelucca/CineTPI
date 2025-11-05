   document.addEventListener('DOMContentLoaded', function() {

    // --- 1. CONFIGURACIÓN ---
    const params = new URLSearchParams(window.location.search);
    const idFuncion = params.get('idFuncion');
    const token = localStorage.getItem('token');
    
    const grid = document.getElementById('butacas-grid');
    const seleccionLista = document.getElementById('seleccion-lista');
    const btnConfirmar = document.getElementById('btn-confirmar');

    let butacasSeleccionadas = []; // Array para guardar las IDs

    if (!idFuncion) {
        alert('Función no válida.');
        window.location.href = 'index.html';
        return; // Salimos temprano si no hay ID
    }

    // --- 2. LLAMAR A LA API ---
    async function cargarButacas() {
        try {
            const response = await fetch(`/api/butacas/funcion/${idFuncion}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (!response.ok) {
                if (response.status === 401) window.location.href = 'login.html';
                throw new Error('No se pudieron cargar las butacas.');
            }
            
            const butacas = await response.json();
            renderButacas(butacas);

        } catch (error) {
            console.error(error);
            grid.innerHTML = "Error al cargar la sala.";
        }
    }

    // --- 3. DIBUJAR LA GRILLA ---
    function renderButacas(butacas) {
        grid.innerHTML = ''; // Limpiar grilla
        butacas.forEach(butaca => {
            const butacaEl = document.createElement('div');
            butacaEl.classList.add('butaca');
            butacaEl.classList.add(butaca.estado); // Añade "Disponible", "Vendida", etc.
            butacaEl.textContent = `${butaca.fila}${butaca.numero}`;
            
            butacaEl.dataset.idButaca = butaca.idButaca;

            if (butaca.estado === 'Disponible') {
                butacaEl.addEventListener('click', toggleSeleccionButaca);
            }

            grid.appendChild(butacaEl);
        });
    }

    // --- 4. LÓGICA DE SELECCIÓN ---
    function toggleSeleccionButaca(event) {
        const butacaEl = event.target;
        const idButaca = parseInt(butacaEl.dataset.idButaca);
        
        if (butacaEl.classList.contains('Seleccionada')) {
            butacaEl.classList.remove('Seleccionada');
            butacasSeleccionadas = butacasSeleccionadas.filter(id => id !== idButaca);
        } else {
            butacaEl.classList.add('Seleccionada');
            butacasSeleccionadas.push(idButaca);
        }
        actualizarResumenSeleccion();
    }

    function actualizarResumenSeleccion() {
        seleccionLista.innerHTML = '';
        butacasSeleccionadas.forEach(id => {
            const butacaEl = grid.querySelector(`.butaca[data-id-butaca="${id}"]`);
            if (butacaEl) { // Agregamos un chequeo por si acaso
                const texto = butacaEl.textContent;
                const li = document.createElement('li');
                li.textContent = `Butaca ${texto}`;
                seleccionLista.appendChild(li);
            }
        });
    }
    
    // --- 5. LÓGICA DE CONFIRMACIÓN ---
    btnConfirmar.addEventListener('click', async function() {
        if (butacasSeleccionadas.length === 0) {
            alert('Debe seleccionar al menos una butaca.');
            return;
        }
        
        const reservaData = {
    IdFuncion: parseInt(idFuncion),  
    IdButacas: butacasSeleccionadas 
        };

        try {
            const response = await fetch('/api/reservas', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(reservaData)
            });

            if (response.ok) {
                const nuevaReserva = await response.json();
                alert(`¡Reserva ${nuevaReserva.idReserva} confirmada con éxito!`);
                window.location.href = 'index.html';
            } else {
                const errorTexto = await response.text();
                alert(`Error al reservar: ${errorTexto}`);
            }

        } catch (error) {
            console.error('Error de red al reservar:', error);
            alert('Error de conexión al intentar reservar.');
        }
    }); 

    // --- INICIAR ---
    cargarButacas(); // Esta línea ahora sí se ejecutará

}); 