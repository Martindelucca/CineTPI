// === CONFIGURACIÓN GLOBAL ===
const API_URL = "/api/peliculas";
const token = localStorage.getItem("token");

// === CARGAR PELÍCULAS ===
async function cargarPeliculas() {
    const tbody = document.getElementById("tbody-peliculas");
    tbody.innerHTML = "<tr><td colspan='6'>Cargando...</td></tr>";

    try {
        const response = await fetch(API_URL, {
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (!response.ok) throw new Error("Error al cargar películas.");

        const data = await response.json();
        renderPeliculas(data);

    } catch (err) {
        console.error("Error al cargar:", err);
        tbody.innerHTML = "<tr><td colspan='6'>Error al cargar películas.</td></tr>";
    }
}

// === RENDERIZAR PELÍCULAS ===
function renderPeliculas(lista) {
    const tbody = document.getElementById("tbody-peliculas");
    tbody.innerHTML = "";

    if (!lista || lista.length === 0) {
        tbody.innerHTML = "<tr><td colspan='5'>No hay películas registradas.</td></tr>";
        return;
    }

    lista.forEach(p => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${p.idPelicula}</td>
            <td>${p.titulo}</td>
            <td>${p.descripcion || "Sin descripción"}</td>
            <td>${p.fechaLanzamiento ? p.fechaLanzamiento.split("T")[0] : "—"}</td>
            <td>
                <button class="btn-action edit" onclick="editarPelicula(${p.idPelicula})">✏️</button>
                <button class="btn-action delete" onclick="eliminarPelicula(${p.idPelicula})">🗑️</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

// === GUARDAR PELÍCULA ===
async function guardarPelicula() {
    const id = document.getElementById("pelicula-id").value;
    const titulo = document.getElementById("titulo").value.trim();
    const descripcion = document.getElementById("descripcion").value.trim();
    const fecha = document.getElementById("fecha-lanzamiento").value;

    if (!titulo || !fecha) {
        alert("Por favor, completá título y fecha de estreno.");
        return;
    }

    const esNuevo = id === "" || id === "0";
    const method = esNuevo ? "POST" : "PUT";
    const url = esNuevo ? API_URL : `${API_URL}/${id}`;

    const body = esNuevo
        ? { Titulo: titulo, Descripcion: descripcion, FechaLanzamiento: fecha }
        : { IdPelicula: parseInt(id), Titulo: titulo, Descripcion: descripcion, FechaLanzamiento: fecha };

    try {
        const response = await fetch(url, {
            method,
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(body)
        });

        if (!response.ok) throw new Error(await response.text());

        await cargarPeliculas();
        limpiarFormulario();
        alert(esNuevo ? "Película agregada correctamente." : "Película actualizada correctamente.");

    } catch (err) {
        console.error("Error al guardar:", err);
        alert("Error al guardar la película.");
    }
}

// === EDITAR ===
async function editarPelicula(id) {
    try {
        const response = await fetch(`${API_URL}/${id}`, {
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (!response.ok) throw new Error("Error al obtener película.");

        const data = await response.json();

        document.getElementById("pelicula-id").value = data.idPelicula;
        document.getElementById("titulo").value = data.titulo;
        document.getElementById("descripcion").value = data.descripcion ?? "";
        document.getElementById("fecha-lanzamiento").value = data.fechaLanzamiento.split("T")[0];

    } catch (err) {
        console.error("Error al editar:", err);
        alert("No se pudo cargar la película para editar.");
    }
}

// === ELIMINAR ===
async function eliminarPelicula(id) {
    if (!confirm("¿Seguro que querés eliminar esta película?")) return;

    try {
        const response = await fetch(`${API_URL}/${id}`, {
            method: "DELETE",
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (!response.ok) throw new Error(await response.text());

        await cargarPeliculas();
        alert("Película eliminada correctamente.");

    } catch (err) {
        console.error("Error al eliminar:", err);
        alert("Error al eliminar película.");
    }
}

// === LIMPIAR ===
function limpiarFormulario() {
    document.getElementById("form-pelicula").reset();
    document.getElementById("pelicula-id").value = "";
}

// === EVENTOS ===
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("btn-guardar").addEventListener("click", guardarPelicula);
    document.getElementById("btn-cancelar").addEventListener("click", limpiarFormulario);
    cargarPeliculas();
});
