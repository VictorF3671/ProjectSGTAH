import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

// páginas
import Login         from '@/pages/Auth/Login.vue';
import MenuTemplate  from '@/pages/MenuTemplate.vue';
import NewProject    from '@/pages/Project/NewProject.vue';
import Dashboard     from '@/pages/Dashboard/Dashboard.vue';
import NewTask       from '@/pages/Task/NewTask.vue'
import ProjectDetails from '@/pages/Project/ProjectDetails' 
import NewUser       from '@/pages/User/NewUser.vue'

const routes: RouteRecordRaw[] = [
  { path: '/',         redirect: '/login' },
  { path: '/login',    name: 'Login', component: Login },

  {
    path: '/menu-template',
    component: MenuTemplate,
    //meta: { requiresAuth: true },
    children: [
     
      { path: '',            redirect: '/menu-template/new-project' },

      {
        path: 'new-project',
        name: 'NewProject',
        component: NewProject,
      },
      {
        path: 'dashboard',
        name: 'Dashboard',
        component: Dashboard,
      },
      {
        path: 'new-user',
        name: 'NewUser',
        component: NewUser,
      },
       {
        path: 'projects/:projectId',
        name: 'ProjectDetails',
        component: ProjectDetails,
        props: true
      },

      // criar nova task dentro de um project
      {
        path: 'projects/:projectId/tasks/new',
        name: 'NewTask',
        component: NewTask,
        props: true
      }
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