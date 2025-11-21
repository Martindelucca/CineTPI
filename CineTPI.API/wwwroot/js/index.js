document.addEventListener('DOMContentLoaded', async () => {

    const token = localStorage.getItem('token');

    // Solo eliminar el botón "Cerrar sesión" si no hay token
    const btnLogout = document.querySelector('.btn-logout');
    const btnAdmin = document.querySelector('a[href="admin.html"]');

    if (!token) {
        if (btnLogout) btnLogout.style.display = "none";
        if (btnAdmin) btnAdmin.style.display = "none";
    }

    const contenedor = document.getElementById('cartelera-container');

    try {
        const resp = await fetch('/api/peliculas/cartelera', {
            headers: token ? { 'Authorization': `Bearer ${token}` } : {}
        });

        if (resp.status === 401) {
            contenedor.innerHTML = `<p class="loading">Inicia sesión para ver la cartelera.</p>`;
            return;
        }

        if (!resp.ok) throw new Error('Error al cargar la cartelera');

        const peliculas = await resp.json();

        if (peliculas.length === 0) {
            contenedor.innerHTML = '<p>No hay películas en cartelera actualmente.</p>';
            return;
        }

        contenedor.innerHTML = '';
        peliculas.forEach(p => {
            contenedor.innerHTML += `
        <div class="card">
          <h3>${p.titulo}</h3>
          <p>${p.descripcion ?? 'Sin descripción disponible'}</p>
          <button class="btn-funciones" data-id="${p.idPelicula}">Ver funciones</button>
        </div>
      `;
        });

        document.querySelectorAll('.btn-funciones').forEach(btn => {
            btn.addEventListener('click', e => {
                const id = e.target.dataset.id;
                window.location.href = `funciones.html?id=${id}`;
            });
        });

    } catch (err) {
        console.error(err);
        contenedor.innerHTML = '<p>Error al cargar cartelera.</p>';
    }
});
