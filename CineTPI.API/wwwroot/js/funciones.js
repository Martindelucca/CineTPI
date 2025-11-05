// Esperamos a que todo el HTML esté cargado
document.addEventListener('DOMContentLoaded', function() {
    
    // 1. Obtenemos el ID de la película de la URL
    // (Ej: funciones.html?id=5)
    const params = new URLSearchParams(window.location.search);
    const idPelicula = params.get('id');

    if (!idPelicula) {
        alert('No se especificó ninguna película.');
        window.location.href = 'index.html';
        return;
    }

    const funcionesList = document.getElementById('funciones-list');
    const peliculaTitulo = document.getElementById('pelicula-titulo');
    const token = localStorage.getItem('token'); // Obtenemos el token una sola vez
    
    // (Opcional: podrías hacer un fetch a /api/peliculas/{id}
    // para obtener el título y ponerlo en el h2)
    peliculaTitulo.textContent = `Funciones para la Película (ID: ${idPelicula})`;

    async function cargarInfoPelicula() {
        try {
            // Llamamos al endpoint de PeliculasController
            const response = await fetch(`/api/peliculas/${idPelicula}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (response.ok) {
                const pelicula = await response.json();
                // ¡Actualizamos el H2 con el título real!
                peliculaTitulo.textContent = `Funciones para: ${pelicula.titulo}`;
            } else {
                // Si falla, dejamos el ID como estaba
                peliculaTitulo.textContent = `Funciones para la Película (ID: ${idPelicula})`;
            }
        } catch (error) {
            console.error('Error al cargar info de la película:', error);
        }
    }
    // 2. Cargar las funciones
    async function cargarFunciones() {
        const token = localStorage.getItem('token');

        try {
            // 3. ¡Llamamos a nuestro NUEVO endpoint!
            const response = await fetch(`/api/funciones/pelicula/${idPelicula}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (response.ok) {
                const funciones = await response.json();
                renderFunciones(funciones);
            } else if (response.status === 401) {
                alert('Tu sesión ha expirado.');
                localStorage.removeItem('token'); // Limpiamos el token malo
                window.location.href = 'login.html';
            } else {
                throw new Error(`Error: ${response.statusText}`);
            }

        } catch (error) {
            console.error('Error al cargar funciones:', error);
            funcionesList.innerHTML = '<p>No se pudieron cargar las funciones.</p>';
        }
    }

    // 4. "Dibujar" las funciones en el HTML
    function renderFunciones(funciones) {
        if (funciones.length === 0) {
            funcionesList.innerHTML = '<p>No hay funciones programadas para esta película.</p>';
            return;
        }
        
        let html = '<ul>';
        funciones.forEach(funcion => {
            const nombreSala = funcion.idSalaNavigation?.nombre || 'Sala no disponible';
            const horario = funcion.idHorarioNavigation?.horario || 'Horario no disponible';
            // ¡Esta data viene de la BBDD!
            // (Necesitás ajustar las propiedades a las de tu entidad 'Funcion')
            html += `
                <li>
                    <strong>Fecha:</strong> ${new Date(funcion.fecha).toLocaleDateString()}
                    <strong>Sala:</strong> ${nombreSala} 
                    <strong>Horario:</strong> ${horario}
                    <br>
                    <a href="butacas.html?idFuncion=${funcion.idFuncion}">Seleccionar Butacas</a>
                </li>
            `;
        });
        html += '</ul>';
        funcionesList.innerHTML = html;
    }

    // 5. Iniciar la carga
    cargarFunciones();
    cargarInfoPelicula();
});