<template>
  <v-container fluid class="pa-4">
    <!-- Cabeçalho com nome do projeto -->
    <v-row>
      <v-col cols="12">
        <h1 class="text-h4">{{ project?.name }}</h1>
      </v-col>
    </v-row>

    <!-- Lista de tasks -->
    <v-row>
      <v-col cols="12" md="8" offset-md="2">
        <v-expansion-panels>
          <v-expansion-panel
            v-for="task in tasks"
            :key="task.id"
          >
            <v-expansion-panel-title class="d-flex align-center justify-space-between">
              {{ task.name }}
              <v-btn icon color="primary" @click.stop="openDialog(task)">
                <v-icon>mdi-play-circle</v-icon>
              </v-btn>
            </v-expansion-panel-title>

            <v-expansion-panel-text>
              <v-list dense two-line>
                <v-list-item
                  v-for="tt in timeTrackers[task.id] || []"
                  :key="tt.id"
                >
                  <v-list-item-content>
                    <v-list-item-title>
                      {{ formatDate(tt.startDate) }} → {{ formatDate(tt.endDate) }}
                    </v-list-item-title>
                    <v-list-item-subtitle>
                      {{ tt.durationHours.toFixed(2) }} horas
                    </v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
                <v-list-item v-if="!(timeTrackers[task.id]?.length)">
                  <v-list-item-content>
                    <v-list-item-title class="text--disabled">
                      Sem apontamentos
                    </v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </v-expansion-panel-text>
          </v-expansion-panel>
        </v-expansion-panels>
      </v-col>
    </v-row>

     <v-dialog v-model="dialog" max-width="500px">
    <v-card>
      <v-card-title>Criar Apontamento</v-card-title>
      <v-card-text>
        <v-form ref="formRef" lazy-validation v-model="valid">
          <!-- Select de Collaborator -->
          <v-select
            v-model="form.collaboratorId"
            :items="collaborators"
            item-title="userName"
            item-value="id"
            label="Colaborador"
            :rules="[v => !!v || 'Selecione um colaborador']"
            required
          />

          <!-- Campos de data/hora -->
          <v-text-field
            v-model="form.startDate"
            label="Início"
            type="datetime-local"
            :rules="[v => !!v || 'Obrigatório']"
          />
          <v-text-field
            v-model="form.endDate"
            label="Término"
            type="datetime-local"
            :rules="[v => !!v || 'Obrigatório']"
          />
          <v-text-field
            v-model="form.timeZone"
            label="Time Zone"
            hint="ex: America/Sao_Paulo"
            persistent-hint
            :rules="[v => !!v || 'Obrigatório']"
          />
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn text @click="dialog = false">Cancelar</v-btn>
        <v-btn :disabled="!valid" color="primary" @click="submit()">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted, reactive } from 'vue'
import { useRoute } from 'vue-router'

import { getProjectById, type IProject } from '@/api/ProjectServices'
import { getAllTask, type ITask }       from '@/api/TaskServices'
import {
  getAllTimeTrackers,
  createTimeTracker,
  type ITimeTracker,
  type ITimeTrackerCreate
} from '@/api/TimeTrackerServices'
import {
  getAllCollaborators,
  type ICollaborator
} from '@/api/CollaboratorServices'  

const route         = useRoute()


const props = defineProps<{ projectId: string }>()
const projectId = Number(props.projectId)


const project       = ref<IProject|null>(null)
const tasks         = ref<ITask[]>([])
const timeTrackers  = ref<Record<number, ITimeTracker[]>>({})
const collaborators = ref<ICollaborator[]>([])

const dialog        = ref(false)
const valid         = ref(false)
const selectedTask  = ref<ITask|null>(null)

//
const formRef = ref()
const form = reactive({
  collaboratorId: null as number|null,
  startDate:      '',
  endDate:        '',
  timeZone:       Intl.DateTimeFormat().resolvedOptions().timeZone
})

async function loadCollaborators() {
  collaborators.value = await getAllCollaborators()
}

async function loadProject() {
  project.value = await getProjectById(projectId)
}

async function loadTasks() {
  tasks.value = await getAllTask(projectId)    
  await Promise.all(tasks.value.map(t => loadTimeTrackers(t.id)))
}

async function loadTimeTrackers(taskId: number) {
  timeTrackers.value = {
    ...timeTrackers.value,
    [taskId]: await getAllTimeTrackers(taskId)
  }
}

function openDialog(task: ITask) {
  selectedTask.value = task
  form.collaboratorId = null
  form.startDate = new Date().toISOString().slice(0,16)
  form.endDate   = new Date().toISOString().slice(0,16)
  dialog.value = true
}

async function submit() {
  if (!selectedTask.value || form.collaboratorId == null) return

  const dto: ITimeTrackerCreate = {
    taskId:         selectedTask.value.id,
    collaboratorId: form.collaboratorId,
    startDate:      new Date(form.startDate).toISOString(),
    endDate:        new Date(form.endDate).toISOString(),
    timeZoneId:     form.timeZone
  } 
  
  await createTimeTracker(dto)
  await loadTimeTrackers(selectedTask.value.id)
  dialog.value = false
}

function formatDate(iso: string) {
  return new Date(iso).toLocaleString('pt-BR', {
    day: '2-digit', month: '2-digit', year: 'numeric',
    hour: '2-digit', minute: '2-digit'
  })
}

onMounted(async () => {
  await Promise.all([
    loadCollaborators(),
    loadProject(),
    loadTasks()
  ])
})
</script>