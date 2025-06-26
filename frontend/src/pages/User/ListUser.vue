<template>
  <v-container fluid class="pa-4">
    <v-row>
      <v-col cols="12">
        <h1 class="text-h4">Lista de Usuários</h1>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-data-table :headers="headers" :items="users" :loading="loading" class="elevation-1">
          <template #item.createdAt="{ item }">
            {{ formatDate(item.createdAt) }}
          </template>

          <template #item.updatedAt="{ item }">
            {{ formatDate(item.updatedAt) }}
          </template>

          <template #item.action="{ item }">

            <v-btn v-if="!isCollaborator(item.id)" icon color="primary" density="compact"
              @click="makeCollaborator(item.id)">
              <v-icon size="20">mdi-account-plus</v-icon>
            </v-btn>

            <v-icon v-else size="20" color="green" title="Já é colaborador">
              mdi-check-circle
            </v-icon>
          </template>
        </v-data-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { getAllUsers, type IUser } from '@/api/UserServices'
import {
  getAllCollaborators,
  createCollaborator,
  type ICollaborator
} from '@/api/CollaboratorServices'

const users = ref<IUser[]>([])
const collaborators = ref<ICollaborator[]>([])
const loading = ref(false)



import type { DataTableHeader } from 'vuetify'
const headers = ref<DataTableHeader<IUser>[]>([
  {
    key:      'id',
    title:    'ID',
    align:    'start',   // só aceita 'start' | 'center' | 'end'
    sortable: true,
  },
  {
    key:      'username',
    title:    'Usuário',
    align:    'start',
    sortable: true,
  },
  {
    key:      'createdAt',
    title:    'Criado em',
    align:    'start',
    sortable: true,
  },
  {
    key:      'updatedAt',
    title:    'Atualizado',
    align:    'start',
    sortable: true,
  },
  {
    key:      'action',     // coluna de ações
    title:    'Colaborador',
    align:    'start',
    sortable: false,
  },
])
const collaboratorUserIds = computed(() =>
  new Set(collaborators.value.map(c => c.userId))
)


function isCollaborator(userId: number): boolean {
  return collaboratorUserIds.value.has(userId)
}

function formatDate(value: string | Date): string {
  const d = typeof value === 'string' ? new Date(value) : value
  return d.toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

async function loadUsers() {
  loading.value = true
  users.value = await getAllUsers()
  loading.value = false
}

async function loadCollaborators() {
  collaborators.value = await getAllCollaborators()
}

async function makeCollaborator(userId: number) {
  try {
    await createCollaborator({ userId })
    await loadCollaborators()
  } catch (err) {
    console.error(err)
    alert('Falha ao criar colaborador.')
  }
}

onMounted(async () => {
  await Promise.all([loadUsers(), loadCollaborators()])
})
</script>