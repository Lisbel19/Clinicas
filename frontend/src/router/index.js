import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/Home.vue';
import Lobby from '../views/Lobby.vue';
import Clinicas from '../views/Clinicas.vue';
import Especialidades from '../views/Especialidades.vue';
import Usuarios from '../views/Usuarios.vue';
import Medicos from '../views/Medicos.vue';
import Pacientes from '../views/Pacientes.vue';
import Citas from '../views/Citas.vue';
import MisCitas from '../views/MisCitas.vue';

const routes = [
    { 
      path: '/', 
      name: 'Home', 
      component: Home,
      meta: { requiresAuth: false }
    },
    // Rutas de Administrador (acceso completo)
    { 
      path: '/clinicas', 
      name: 'Clinicas', 
      component: Clinicas,
      meta: { requiresAuth: true, allowedRoles: ['administrador'] }
    },
    { 
      path: '/especialidades', 
      name: 'Especialidades', 
      component: Especialidades,
      meta: { requiresAuth: true, allowedRoles: ['administrador'] }
    },
    { 
      path: '/usuarios', 
      name: 'Usuarios', 
      component: Usuarios,
      meta: { requiresAuth: true, allowedRoles: ['administrador'] }
    },
    { 
      path: '/medicos', 
      name: 'Medicos', 
      component: Medicos,
      meta: { requiresAuth: true, allowedRoles: ['administrador'] }
    },
    { 
      path: '/pacientes', 
      name: 'Pacientes', 
      component: Pacientes,
      meta: { requiresAuth: true, allowedRoles: ['administrador'] }
    },
    { 
      path: '/citas', 
      name: 'Citas', 
      component: Citas,
      meta: { requiresAuth: true, allowedRoles: ['administrador'] }
    },
    
    // Rutas compartidas (administrador + paciente)
    { 
      path: '/mis-citas', 
      name: 'MisCitas', 
      component: MisCitas,
      meta: { requiresAuth: true, allowedRoles: ['administrador', 'paciente'] }
    },
    // Rutas compartidas (administrador + medico)
    { 
      path: '/citas-medicos', 
      name: 'CitasMedicos', 
      component: () => import('../views/CitasMedicos.vue'),
      meta: { requiresAuth: true, allowedRoles: ['administrador', 'medico'] }
    },
    // Rutas compartidas (admin + medico + paciente)
    { 
        path: '/lobby', 
        name: 'Lobby', 
        component: Lobby,
        meta: { requiresAuth: true, allowedRoles: ['administrador', 'medico', 'paciente'] }
      },
    // Ruta para manejar errores 404
    { 
      path: '/:pathMatch(.*)*', 
      redirect: '/' 
    }
  ];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem('authToken');
  const userRole = localStorage.getItem('userRole');
  
  // Verificar si la ruta requiere autenticación
  if (to.meta.requiresAuth) {
    if (!isAuthenticated) {
      return next('/');
    }
    
    if (to.meta.allowedRoles && !to.meta.allowedRoles.includes(userRole)) {
      // Administrador puede acceder a todo
      if (userRole === 'administrador') {
        return next();
      }
      
      switch(userRole) {
        default: return next('/');
      }
    }
  }
  
  // Si está autenticado y va al home, redirigir según rol
  if (to.path === '/' && isAuthenticated) {
    switch(userRole) {
      default: return next('/lobby');
    }
  }
  
  next();
});

export default router;