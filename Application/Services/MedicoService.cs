using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public MedicoService(IMedicoRepository medicoRepository, IUsuarioRepository usuarioRepository)
        {
            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            return await _medicoRepository.GetAllAsync();
        }

        public async Task<Medico?> GetByIdAsync(int id)
        {
            return await _medicoRepository.GetByIdAsync(id);
        }

        public async Task<Medico?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _medicoRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId)
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return medicos.Where(m => m.EspecialidadId == especialidadId);
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosMedicosAsync()
        {
            return await _usuarioRepository.GetUsuariosMedicosAsync();
        }

        public async Task<bool> ExistsByUsuarioIdAsync(int usuarioId)
        {
            var medico = await _medicoRepository.GetByUsuarioIdAsync(usuarioId);
            return medico != null;
        }

        public async Task<Medico> CreateAsync(Medico medico, string correoUsuario)
        {
            // Verificar que el usuario existe y es de tipo médico
            var usuario = await _usuarioRepository.GetByCorreoAsync(correoUsuario);
            if (usuario == null || usuario.TipoUsuario != "medico")
            {
                throw new InvalidOperationException("El usuario no existe o no es de tipo médico");
            }

            // Verificar que el usuario no tenga ya un médico asociado
            if (await ExistsByUsuarioIdAsync(usuario.Id))
            {
                throw new InvalidOperationException("El usuario ya tiene un perfil médico asociado");
            }

            medico.UsuarioId = usuario.Id;
            medico.Correo = usuario.Correo; // Establecer el correo desde el usuario

            return await _medicoRepository.AddAsync(medico);
        }

        public async Task UpdateAsync(Medico medico)
        {
            // No permitir actualizar el correo ya que viene del usuario
            var medicoExistente = await _medicoRepository.GetByIdAsync(medico.Id);
            if (medicoExistente != null)
            {
                medico.Correo = medicoExistente.Correo;
                medico.UsuarioId = medicoExistente.UsuarioId;
            }

            await _medicoRepository.UpdateAsync(medico);
        }

        public async Task DeleteAsync(int id)
        {
            await _medicoRepository.DeleteAsync(id);
        }
    }
}