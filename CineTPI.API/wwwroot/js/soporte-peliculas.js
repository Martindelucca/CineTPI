document.addEventListener('DOMContentLoaded', function () {

    // --- 1. CONFIGURACIÓN ---
    const token = localStorage.getItem('token');
    const tbody = document.getElementById('tbody-peliculas');
    const form = document.getElementById('form-pelicula');
    const btnGuardar = document.getElementById('btn-guardar');
    const btnCancelar = document.getElementById('btn-cancelar');

    // Inputs del formulario
    const inputId = document.getElementById('pelicula-id');
    const inputTitulo = document.getElementById('titulo');
    const inputDescripcion = document.getElementById('descripcion');
    const inputFecha = document.getElementById('fecha-lanzamiento');

    const API_URL = '/api/peliculas';

    // --- 2. FUNCIÓN DE CARGA (GET Global) ---
    async function cargarPeliculas() {
        tbody.innerHTML = '<tr><td colspan="3">Cargando...</td></tr>';
        try {
            const response = await fetch(API_URL, {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            if (!response.ok) {
                if (response.status === 401) window.location.href = 'login.html';
                throw new Error('Error al cargar películas.');
            }
            const peliculas = await response.json();
            renderTabla(peliculas);
        } catch (error) {
            console.error(error);
            tbody.innerHTML = '<tr><td colspan="3">Error al cargar.</td></tr>';
        }
    }

    // --- 3. FUNCIÓN DE RENDERIZADO ---
    function renderTabla(peliculas) {
        tbody.innerHTML = '';
        peliculas.forEach(pelicula => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${pelicula.idPelicula}</td>
                <td>${pelicula.titulo}</td>
                <td>
                    <button class="btn-editar" data-id="${pelicula.idPelicula}">Editar</button>
                    <button class="btn-borrar" data-id="${pelicula.idPelicula}">Borrar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    }
    delete pelicula.idPelicula;

if (pelicula.idPelicula === 0 || pelicula.idPelicula === "0") {
    delete pelicula.idPelicula; // 🔥 Esto elimina el campo del JSON antes de enviarlo
}

    // --- 4. LÓGICA DEL FORMULARIO (POST y PUT) ---
    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        const id = parseInt(inputId.value); // 0 si es nuevo
        const esNuevo = id === 0;

        let peliculaDto = {};
        let method = '';
        let url = '';

        if (esNuevo) {
            // --- ALTA (POST) ---
            method = 'POST';
            url = API_URL;
            // ✅ CAMBIO: No enviamos el idPelicula en el JSON
            peliculaDto = {
                titulo: inputTitulo.value,
                descripcion: inputDescripcion.value,
                fechaLanzamiento: inputFecha.value
            };
        } else {
            // --- MODIFICACIÓN (PUT) ---
            method = 'PUT';
            url = `${API_URL}/${id}`;
            peliculaDto = {
                idPelicula: id,
                titulo: inputTitulo.value,
                descripcion: inputDescripcion.value,
                fechaLanzamiento: inputFecha.value
            };
        }

        try {
            const response = await fetch(url, {
                method: method,
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(peliculaDto)
            });

            if (response.ok) {
                alert(`Película ${esNuevo ? 'creada' : 'actualizada'} con éxito.`);
                resetFormulario();
                cargarPeliculas(); // Recargamos la tabla
            } else {
                const error = await response.text(); // ✅ CAMBIO: texto directo, no JSON
                alert(`Error: ${error}`);
            }
        } catch (error) {
            console.error('Error al guardar:', error);
            alert('Error de conexión con el servidor.');
        }
    });

    // --- 5. LÓGICA DE BOTONES (EDITAR y BORRAR) ---
    tbody.addEventListener('click', async function (e) {

        // Botón BORRAR (DELETE)
        if (e.target.classList.contains('btn-borrar')) {
            const id = e.target.dataset.id;
            if (confirm(`¿Está seguro que desea borrar la película ID ${id}?`)) {
                try {
                    const response = await fetch(`${API_URL}/${id}`, {
                        method: 'DELETE',
                        headers: { 'Authorization': `Bearer ${token}` }
                    });
                    if (response.ok) {
                        alert('Película borrada.');
                        cargarPeliculas(); // Recargamos la tabla
                    } else {
                        alert('Error al borrar.');
                    }
                } catch (error) {
                    console.error('Error al borrar:', error);
                }
            }
        }

        // Botón EDITAR (Prepara el formulario)
        if (e.target.classList.contains('btn-editar')) {
            const id = e.target.dataset.id;
            const response = await fetch(`${API_URL}/${id}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            if (!response.ok) { alert('No se pudo cargar la película.'); return; }

            const pelicula = await response.json();

            // Llenamos el formulario
            inputId.value = pelicula.idPelicula;
            inputTitulo.value = pelicula.titulo;
            inputDescripcion.value = pelicula.descripcion;
            inputFecha.value = pelicula.fechaLanzamiento.split('T')[0]; // Fecha ISO → formato input

            btnGuardar.textContent = 'Actualizar';
            btnCancelar.style.display = 'inline-block';
        }
    });

    // Botón Cancelar (Limpia el formulario)
    btnCancelar.addEventListener('click', resetFormulario);

    function resetFormulario() {
        form.reset();
        inputId.value = '0';
        btnGuardar.textContent = 'Guardar';
        btnCancelar.style.display = 'none';
    }

    // --- INICIAR ---
    cargarPeliculas();
});
