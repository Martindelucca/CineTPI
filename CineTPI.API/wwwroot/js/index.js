// Esperamos a que todo el HTML (index.html) esté cargado
document.addEventListener('DOMContentLoaded', function() {

    // --- 1. Saludar al Usuario y Configurar Logout ---
    const welcomeMessage = document.getElementById('welcome-message');
    const logoutButton = document.getElementById('logout-button');
    const peliculasList = document.getElementById('peliculas-list');
    
    // Obtenemos el nombre guardado en el login
    const nombreUsuario = localStorage.getItem('usuarioNombre');
    if (nombreUsuario) {
        welcomeMessage.textContent = `Bienvenido, ${nombreUsuario}`;
    }

    // Configurar el botón de Cerrar Sesión
    logoutButton.addEventListener('click', function() {
        // Borramos los datos de sesión del navegador
        localStorage.removeItem('token');
        localStorage.removeItem('usuarioNombre');
        
        // Lo mandamos al login
        window.location.href = 'login.html';
    });

    // --- 2. Cargar las Películas (Llamada 'fetch' AUTENTICADA) ---
    async function cargarPeliculasEnCartelera() {
        
        // ¡¡ESTA ES LA PARTE CLAVE DEL TPI!!
        
        // a. Obtenemos el token que guardamos en el login
        const token = localStorage.getItem('token');

        try {
            // b. Hacemos el 'fetch' al endpoint protegido
            const response = await fetch('/api/peliculas/cartelera', {
                method: 'GET',
                headers: {
                    // c. ¡Añadimos el token a la cabecera 'Authorization'!
                    // El formato es "Bearer" (espacio) y luego el token.
                    'Authorization': `Bearer ${token}`
                }
            });

            // d. Verificamos la respuesta
            if (response.ok) {
                // Éxito (200 OK)
                const peliculas = await response.json();
                renderPeliculas(peliculas);
            } else if (response.status === 401) {
                // Error 401 (Token inválido o expirado)
                alert('Tu sesión ha expirado. Por favor, volvé a iniciar sesión.');
                logoutButton.click(); // Forzamos el logout
            } else {
                // Otro error (404, 500...)
                throw new Error(`Error del servidor: ${response.status}`);
            }

        } catch (error) {
            console.error('Error al cargar películas:', error);
            peliculasList.innerHTML = '<p>No se pudieron cargar las películas. Intente más tarde.</p>';
        }
    }

    // --- 3. Función para "dibujar" las películas en el HTML ---
    function renderPeliculas(peliculas) {
        if (peliculas.length === 0) {
            peliculasList.innerHTML = '<p>No hay películas en cartelera en este momento.</p>';
            return;
        }

        let html = '<ul>';
        peliculas.forEach(pelicula => {
            html += `
                <li>
                    <h3>${pelicula.titulo}</h3>
                    <a href="funciones.html?id=${pelicula.idPelicula}">Ver Funciones</a>
                </li>
            `;
        });
        html += '</ul>';
        peliculasList.innerHTML = html;
    }

    // --- 4. Iniciar la carga ---
    cargarPeliculasEnCartelera();
});