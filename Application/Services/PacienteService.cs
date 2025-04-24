using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public PacienteService(
            IPacienteRepository pacienteRepository,
            IUsuarioRepository usuarioRepository)
        {
            _pacienteRepository = pacienteRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _pacienteRepository.GetAllAsync();
        }

        public async Task<Paciente?> GetByIdAsync(int id)
        {
            return await _pacienteRepository.GetByIdAsync(id);
        }

        public async Task<bool> ExistsByUsuarioIdAsync(int usuarioId)
        {
            var paciente = await _pacienteRepository.GetByUsuarioIdAsync(usuarioId);
            return paciente != null;
        }

        public async Task<Paciente> CreateAsync(Paciente paciente, string correoUsuario)
        {
            // Verificar que el usuario existe y es de tipo paciente
            var usuario = await _usuarioRepository.GetByCorreoAsync(correoUsuario);
            if (usuario == null || usuario.TipoUsuario != "paciente")
            {
                throw new InvalidOperationException("El usuario no existe o no es de tipo paciente");
            }

            // Verificar que el usuario no tenga ya un paciente asociado
            if (await ExistsByUsuarioIdAsync(usuario.Id))
            {
                throw new InvalidOperationException("El usuario ya tiene un perfil de paciente asociado");
            }

            // Asignar los valores necesarios
            paciente.UsuarioId = usuario.Id;
            paciente.Correo = usuario.Correo;

            return await _pacienteRepository.AddAsync(paciente);
        }

        public async Task UpdateAsync(Paciente paciente)
        {
            // Obtener el paciente existente para asegurar integridad de datos
            var pacienteExistente = await _pacienteRepository.GetByIdAsync(paciente.Id);
            
            if (pacienteExistente == null)
            {
                throw new InvalidOperationException("Paciente no encontrado");
            }

            // Actualizar solo los campos permitidos
            pacienteExistente.Nombre = paciente.Nombre;
            pacienteExistente.Apellido = paciente.Apellido;
            pacienteExistente.Telefono = paciente.Telefono;
            pacienteExistente.Direccion = paciente.Direccion;

            await _pacienteRepository.UpdateAsync(pacienteExistente);
        }

        public async Task DeleteAsync(int id)
        {
            await _pacienteRepository.DeleteAsync(id);
        }
    }
}