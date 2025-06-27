<template>
  <v-app>
    
    <v-navigation-drawer
      v-model="drawer"
      app
      permanent
    >
      <v-list>
        <v-list-item>
          <v-list-item-title class="text-h6">Projeto Inga</v-list-item-title>
        </v-list-item>
      </v-list>

      <v-divider />

      <v-list density="compact" nav>
        <v-list-item
          prepend-icon="mdi-view-dashboard-outline"
          title="Dashboard"
          to="/menu-template/dashboard"
          link
        />
        <v-list-item
          prepend-icon="mdi-archive-edit-outline"
          title="Novo Projeto"
          to="/menu-template/projects/new"
          link
        />
        
         <v-list-group
        v-model="usersOpen"
      >
        <template #activator="{ props }">
          <v-list-item v-bind="props"
          title="Usuarios"
          prepend-icon="mdi-account-group-outline"
          >
          </v-list-item>
        </template>

    
        <v-list-item
          v-for="(item, i) in userItems"
          :key="i"
          :to="item.to"
          link
          :title="item.title"
          :prepend-icon="item.icon"
        >
        </v-list-item>
      </v-list-group>
      </v-list>
    </v-navigation-drawer>


    <v-app-bar app flat>
      <v-app-bar-title>Projeto Inga</v-app-bar-title>
      <v-spacer />
    </v-app-bar>

  
    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from 'vue';


const usersOpen = ref(false)
const userItems = [
  { title: 'Lista Usuário', icon: 'mdi-account-multiple-outline', to: '/menu-template/list-user' },
  { title: 'Novo Usuário',  icon: 'mdi-account-plus-outline',     to: '/menu-template/new-user' }
]

const drawer = ref(true);
</script>
