<template>
  <v-app>
    <!-- Drawer fixado ao lado -->
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
          prepend-icon="mdi-archive-edit-outline"
          title="Novo Projeto"
          to="/menu-template/new-project"
          link
        />
        <v-list-item
          prepend-icon="mdi-view-dashboard-outline"
          title="Dashboard"
          to="/menu-template/dashboard"
          link
        />
        <v-list-item
          prepend-icon="mdi-account-outline"
          title="Novo Usuario"
          to="/menu-template/new-user"
          link
        />
      </v-list>
    </v-navigation-drawer>

    <!-- App bar no topo -->
    <v-app-bar app flat>
      <v-app-bar-title>Projeto Inga</v-app-bar-title>
      <v-spacer />
    </v-app-bar>

    <!-- Área de conteúdo principal -->
    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const drawer = ref(true);
</script>
