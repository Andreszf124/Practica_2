(() => {

    const Categoria = {
        tabla: null,
        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {
            this.tabla = new DataTable('#tblCategoria', {
                ajax: {
                    url: '/Categoria/ObtenerCategorias',
                    dataSrc: '',
                    type: 'GET'
                },
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'name', title: 'Nombre' },
                    { data: 'description', title: 'Descripción' },
                    {
                        data: null,
                        title: 'Acciones',
                        className: 'dt-actions-cell',
                        orderable: false,
                        searchable: false,
                        render: (data, type, row) => {
                            return `<div class="table-actions">
                                        <button type="button" class="btn-action btn-action-edit btn-editar" data-id="${row.id}" aria-label="Editar categoría" title="Editar">
                                            <i class="bi bi-pencil-fill" aria-hidden="true"></i>
                                        </button>
                                        <button type="button" class="btn-action btn-action-delete btn-eliminar" data-id="${row.id}" aria-label="Eliminar categoría" title="Eliminar">
                                            <i class="bi bi-trash3-fill" aria-hidden="true"></i>
                                        </button>
                                    </div>`;
                        }
                    }
                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                },
                pageLength: 10,
                processing: true,
                serverSide: false
            });
        },

        registrarEventos() {
            // Evento para guardar categoría
            $('#btnGuardarCategoria').on('click', () => {
                this.guardarCategoria();
            });

            // Evento para actualizar categoría
            $('#btnEditarCategoria').on('click', () => {
                this.editarCategoria();
            });

            // Eventos de tabla - Eliminar
            $('#tblCategoria').on('click', '.btn-eliminar', (e) => {
                const id = $(e.currentTarget).data('id');
                this.confirmarEliminar(id);
            });

            // Eventos de tabla - Editar
            $('#tblCategoria').on('click', '.btn-editar', (e) => {
                const id = $(e.currentTarget).data('id');
                this.cargarCategoria(id);
            });

            // Limpiar modal al abrir
            $('#modalCrearCategoria').on('show.bs.modal', () => {
                $('#formCrearCategoria')[0].reset();
                $('#mensajeCrearCategoria').addClass('d-none').removeClass('alert-danger alert-success');
            });

            $('#modalEditarCategoria').on('show.bs.modal', () => {
                $('#mensajeEditarCategoria').addClass('d-none').removeClass('alert-danger alert-success');
            });
        },

        guardarCategoria() {
            const form = $('#formCrearCategoria');
            const nombre = $('#formCrearCategoria #name').val();
            const descripcion = $('#formCrearCategoria #description').val();

            if (!nombre) {
                this.mostrarMensaje('#mensajeCrearCategoria', 'El nombre es requerido', 'danger');
                return;
            }

            const datos = {
                name: nombre,
                description: descripcion
            };

            $.ajax({
                url: '/Categoria/CrearCategoria',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                success: (response) => {
                    if (response.success || response === true) {
                        this.mostrarMensaje('#mensajeCrearCategoria', 'Categoría creada exitosamente', 'success');
                        setTimeout(() => {
                            bootstrap.Modal.getInstance(document.getElementById('modalCrearCategoria')).hide();
                            this.tabla.ajax.reload();
                            form[0].reset();
                        }, 1500);
                    } else {
                        this.mostrarMensaje('#mensajeCrearCategoria', 'Error al crear la categoría', 'danger');
                    }
                },
                error: (xhr) => {
                    const mensaje = xhr.responseJSON?.message || 'Error en la solicitud';
                    this.mostrarMensaje('#mensajeCrearCategoria', mensaje, 'danger');
                }
            });
        },

        cargarCategoria(id) {
            $.ajax({
                url: `/Categoria/ObtenerCategoriaPorId/${id}`,
                type: 'GET',
                success: (categoria) => {
                    $('#formEditarCategoria #id').val(categoria.id);
                    $('#formEditarCategoria #name').val(categoria.name);
                    $('#formEditarCategoria #description').val(categoria.description);

                    const modal = new bootstrap.Modal(document.getElementById('modalEditarCategoria'));
                    modal.show();
                },
                error: (xhr) => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No se pudo cargar la categoría'
                    });
                }
            });
        },

        editarCategoria() {
            const id = $('#formEditarCategoria #id').val();
            const nombre = $('#formEditarCategoria #name').val();
            const descripcion = $('#formEditarCategoria #description').val();

            if (!nombre) {
                this.mostrarMensaje('#mensajeEditarCategoria', 'El nombre es requerido', 'danger');
                return;
            }

            const datos = {
                id: parseInt(id),
                name: nombre,
                description: descripcion
            };

            $.ajax({
                url: '/Categoria/ActualizarCategoria',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                success: (response) => {
                    if (response.success || response === true) {
                        this.mostrarMensaje('#mensajeEditarCategoria', 'Categoría actualizada exitosamente', 'success');
                        setTimeout(() => {
                            bootstrap.Modal.getInstance(document.getElementById('modalEditarCategoria')).hide();
                            this.tabla.ajax.reload();
                        }, 1500);
                    } else {
                        this.mostrarMensaje('#mensajeEditarCategoria', 'Error al actualizar la categoría', 'danger');
                    }
                },
                error: (xhr) => {
                    const mensaje = xhr.responseJSON?.message || 'Error en la solicitud';
                    this.mostrarMensaje('#mensajeEditarCategoria', mensaje, 'danger');
                }
            });
        },

        confirmarEliminar(id) {
            Swal.fire({
                title: '¿Estás seguro?',
                text: 'No podrás revertir esta acción',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    this.eliminarCategoria(id);
                }
            });
        },

        eliminarCategoria(id) {
            $.ajax({
                url: `/Categoria/EliminarCategoria/${id}`,
                type: 'DELETE',
                success: (response) => {
                    if (response.success || response === true) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Eliminado',
                            text: 'La categoría ha sido eliminada',
                            timer: 1500
                        });
                        this.tabla.ajax.reload();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'No se pudo eliminar la categoría'
                        });
                    }
                },
                error: (xhr) => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error en la solicitud'
                    });
                }
            });
        },

        mostrarMensaje(selector, mensaje, tipo) {
            const elemento = $(selector);
            elemento.removeClass('d-none alert-danger alert-success')
                    .addClass(`alert-${tipo}`)
                    .text(mensaje);
        }
    };

    // Inicializar cuando el documento esté listo
    $(document).ready(() => {
        Categoria.init();
    });

    // Exponer globalmente si es necesario
    window.Categoria = Categoria;
})();
