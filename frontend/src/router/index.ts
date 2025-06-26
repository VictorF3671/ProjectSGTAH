import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

// páginas
import Login from '@/pages/Auth/Login.vue';
import MenuTemplate from '@/pages/MenuTemplate.vue';
import NewProject from '@/pages/Project/NewProject.vue';
import Dashboard from '@/pages/Dashboard/Dashboard.vue';
import NewTask from '@/pages/Task/NewTask.vue'
import ProjectDetails from '@/pages/Project/ProjectDetails'
import NewUser from '@/pages/User/NewUser.vue'
import ListUser from '@/pages/User/ListUser.vue'

const routes: RouteRecordRaw[] = [
  { path: '/', redirect: '/login' },
  { path: '/login', name: 'Login', component: Login },

  {
    path: '/menu-template',
    component: MenuTemplate,
    //meta: { requiresAuth: true },
    children: [

      { path: '', redirect: 'menu-template/dashboard' },

      { path: 'dashboard', name: 'Dashboard', component: Dashboard },

      {
        path: 'projects/new/:projectId?',
        name: 'NewProject',
        component: NewProject,
        props: true
      },

      { path: 'projects/:projectId', name: 'ProjectDetails', component: ProjectDetails, props: true },

      // tasks (create & edit)
      {
        path: 'projects/:projectId/tasks/new/:taskId?',
        name: 'NewTask',
        component: NewTask,
        props: true
      },

      // usuários
      { path: 'new-user', name: 'NewUser', component: NewUser },

      { path: 'list-user', name: 'ListUser', component: ListUser },
    ]
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('token');
  if (to.meta.requiresAuth && !isAuthenticated) {
    return next('/login');
  }
  next();
});

export default router;