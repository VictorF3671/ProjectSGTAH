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
          prepend-icon="mdi-cart-outline"
          title="Novo Projeto"
          to="/menu-template/new-project"
          link
        />
        <v-list-item
          prepend-icon="mdi-cart-outline"
          title="Dashboard"
          to="/menu-template/dashboard"
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
