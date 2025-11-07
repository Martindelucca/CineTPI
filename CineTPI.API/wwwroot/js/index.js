document.addEventListener('DOMContentLoaded', async () => {
  const token = localStorage.getItem('token');
  const btnLogin = document.getElementById('btn-login');
  const btnAdmin = document.getElementById('btn-admin');
  const contenedor = document.getElementById('cartelera-container');

  if (token) {
    btnLogin.style.display = 'none';
    btnAdmin.style.display = 'inline-block';
  }

  try {
    const resp = await fetch('/api/peliculas/cartelera', {
      headers: token ? { 'Authorization': `Bearer ${token}` } : {}
    });

    if (!resp.ok) throw new Error('Error al cargar cartelera');

    const peliculas = await resp.json();

    if (peliculas.length === 0) {
      contenedor.innerHTML = '<p>No hay películas en cartelera actualmente.</p>';
      return;
    }

    contenedor.innerHTML = '';
    peliculas.forEach(p => {
      const card = document.createElement('div');
      card.className = 'card';
      card.innerHTML = `
        <h3>${p.titulo}</h3>
        <p>${p.descripcion ?? 'Sin descripción disponible'}</p>
        <p><strong>Estreno:</strong> ${p.fechaLanzamiento?.split('T')[0] ?? 'N/D'}</p>
        <button class="btn-funciones" data-id="${p.idPelicula}">Ver funciones</button>
      `;
      contenedor.appendChild(card);
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
