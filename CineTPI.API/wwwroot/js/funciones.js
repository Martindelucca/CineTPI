// --- Esperamos a que el HTML cargue completamente ---
document.addEventListener("DOMContentLoaded", async () => {
  const params = new URLSearchParams(window.location.search);
  const idPelicula = params.get("id");
  const lista = document.getElementById("lista-funciones");

  // --- Validamos ID ---
  if (!idPelicula) {
    lista.innerHTML = `<p class="msg-error">No se seleccionó ninguna película.</p>`;
    return;
  }

  // --- Obtenemos token ---
  const token = localStorage.getItem("token");
  if (!token) {
    alert("⚠️ Debes iniciar sesión para ver las funciones.");
    window.location.href = "login.html";
    return;
  }

  // --- Mostramos mensaje de carga ---
  lista.innerHTML = `<p class="msg-cargando">Cargando funciones...</p>`;

  try {
    // --- Llamada a la API ---
    const response = await fetch(`/api/funciones/pelicula/${idPelicula}`, {
      method: "GET",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json"
      }
    });

    // --- Verificación de estado ---
    if (response.status === 401) {
      alert("⚠️ Sesión expirada o no autorizada. Por favor, inicia sesión nuevamente.");
      localStorage.removeItem("token");
      window.location.href = "login.html";
      return;
    }

    if (!response.ok) {
      throw new Error(`Error HTTP ${response.status}`);
    }

    // --- Procesamos las funciones ---
    const funciones = await response.json();

    if (!funciones || funciones.length === 0) {
      lista.innerHTML = `<p class="msg-vacio">No hay funciones disponibles para esta película.</p>`;
      return;
    }

    // --- Render dinámico ---
    lista.innerHTML = funciones.map(f => `
      <div class="card-funcion">
        <h3>🎬 ${f.nombreSala}</h3>
        <p><strong>Fecha:</strong> ${f.fecha}</p>
        <p><strong>Horario:</strong> ${f.horario}</p>
      </div>
    `).join("");

  } catch (error) {
    console.error("Error al cargar funciones:", error);
    lista.innerHTML = `<p class="msg-error">Error al cargar las funciones. Intente más tarde.</p>`;
  }
});

