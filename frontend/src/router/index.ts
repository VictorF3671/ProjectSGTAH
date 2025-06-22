import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

// Layout do Dashboard (que vai conter as rotas internas com a barra lateral e app-bar)
// import Dashboard from '@/pages/Dashboard.vue';

// Páginas
import Login from '@/pages/Auth/Login.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/login', 
  },
  {
    path: '/login',
    name: 'Login',
    component: Login,
  },
  // {
  //   path: '/dashboard',
  //   component: Dashboard,
  //   children: [
  //     {
  //       path: '',
  //       redirect: '/dashboard', 
  //     },
      
  //   ],
  //   //meta: { requiresAuth: true }, 
  // },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

// Guard global de autenticação
router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('token');
  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login');
  } else {
    next();
  }
});

export default router;