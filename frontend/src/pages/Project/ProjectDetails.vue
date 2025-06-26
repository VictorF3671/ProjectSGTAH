<template>
    <v-container fluid class="pa-4">
        <!-- Cabeçalho e totais -->
        <v-row class="align-center mb-4">
            <v-col cols="12" md="6">
                <h1 class="text-h4">{{ project?.name }}</h1>
            </v-col>
            <v-col cols="6" md="3">
                <v-card flat class="pa-2">
                    <div class="d-flex justify-space-between">
                        <span>Hoje:</span>
                        <strong>{{ todayTotal }}</strong>
                    </div>
                </v-card>
            </v-col>
            <v-col cols="6" md="3">
                <v-card flat class="pa-2">
                    <div class="d-flex justify-space-between">
                        <span>Mês:</span>
                        <strong>{{ monthTotal }}</strong>
                    </div>
                </v-card>
            </v-col>
        </v-row>

        <!-- Filtro de colaborador -->
        <v-row class="mb-4">
            <v-col cols="12" md="4">
                <v-select v-model="selectedCollaborator" :items="collaborators" item-title="username" item-value="id"
                    label="Filtrar por colaborador" clearable />
            </v-col>
        </v-row>

        <!-- Lista de tasks com botões -->
        <v-row>
            <v-col cols="12" md="8" offset-md="2">
                <v-expansion-panels>
                    <v-expansion-panel v-for="task in filteredTasks" :key="task.id">
                        <v-expansion-panel-title class="d-flex align-center justify-space-between">
                            {{ task.name }}
                            <div class="d-flex gap-2">
                                <!-- Start/Stop -->
                                <v-btn icon density="compact"
                                    :color="activeTrackerByTask[task.id] ? 'error' : 'success'"
                                    @click.stop="activeTrackerByTask[task.id] ? stop(task) : openDialog(task)">
                                    <v-icon size="20">
                                        {{ activeTrackerByTask[task.id] ? 'mdi-stop-circle' : 'mdi-play-circle' }}
                                    </v-icon>
                                </v-btn>

                                <v-btn icon density="compact" color="primary" @click.stop="editTask(task.id)">
                                    <v-icon size="20">mdi-pencil</v-icon>
                                </v-btn>

                                
                                <v-btn icon density="compact" color="error" @click.stop="onDeleteTask(task.id)">
                                    <v-icon size="20">mdi-delete</v-icon>
                                </v-btn>
                            </div>
                        </v-expansion-panel-title>

                        <v-expansion-panel-text>
                            <v-list dense two-line>
                                <v-list-item v-for="tt in timeTrackers[task.id] || []" :key="tt.id">
                                    <v-list-item-content>
                                        <v-list-item-title>
                                            {{ formatDate(tt.startDate) }} → {{ formatDate(tt.endDate) }}
                                        </v-list-item-title>
                                        <v-list-item-subtitle>
                                            {{ tt.durationHours.toFixed(2) }} horas
                                        </v-list-item-subtitle>
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item v-if="!timeTrackers[task.id]?.length">
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

        <!-- Diálogo para iniciar tracker -->
        <v-dialog v-model="dialog" max-width="400px">
            <v-card>
                <v-card-title>Escolher Colaborador</v-card-title>
                <v-card-text>
                    <v-form ref="formRef" v-model="valid">
                        <v-select v-model="form.collaboratorId" :items="collaborators" item-title="username"
                            item-value="id" label="Colaborador" :rules="[v => !!v || 'Selecione um colaborador']"
                            required />
                    </v-form>
                </v-card-text>
                <v-card-actions>
                    <v-spacer />
                    <v-btn text @click="dialog = false">Cancelar</v-btn>
                    <v-btn :disabled="!valid" color="primary" @click="confirmStart">
                        Iniciar
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <!-- Snackbar -->
        <v-snackbar v-model="snackbar.show" :color="snackbar.color" top timeout="5000">
            {{ snackbar.message }}
            <template #actions>
                <v-btn text @click="snackbar.show = false">Fechar</v-btn>
            </template>
        </v-snackbar>
    </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, computed, reactive } from 'vue'
import { defineProps } from 'vue'
import { useRouter } from 'vue-router'

import { getProjectById, type IProject } from '@/api/ProjectServices'
import { getAllTask, deleteTask, type ITask } from '@/api/TaskServices'
import {
    getAllTimeTrackers,
    createTimeTracker,
    updateTimeTracker,
    getTodayTotal,
    getMonthTotal,
    type ITimeTracker,
    type ITimeTrackerCreate
} from '@/api/TimeTrackerServices'
import {
    getAllCollaborators,
    type ICollaborator
} from '@/api/CollaboratorServices'

const props = defineProps<{ projectId: string }>()
const projectId = Number(props.projectId)
const router = useRouter()

const project = ref<IProject | null>(null)
const tasks = ref<ITask[]>([])
const collaborators = ref<ICollaborator[]>([])
const todayTotal = ref('00:00')
const monthTotal = ref('00:00')

const selectedCollaborator = ref<number | null>(null)
const dialog = ref(false)
const valid = ref(false)
const formRef = ref()
const form = reactive<{ collaboratorId: number | null }>({
    collaboratorId: null
})

const timeTrackers = ref<Record<number, ITimeTracker[]>>({})
const activeTrackerByTask = ref<Record<number, number>>({})

const snackbar = ref({ show: false, message: '', color: 'error' })

const filteredTasks = computed(() => {
    if (selectedCollaborator.value == null) return tasks.value
    return tasks.value.filter(task => {
        const list = timeTrackers.value[task.id] || []
        return list.some(tt => tt.collaboratorId === selectedCollaborator.value)
    })
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
    await refreshTotals()
}

function editTask(taskId: number) {
  router.push(`/menu-template/projects/${projectId}/tasks/new/${taskId}`)
}

async function loadTimeTrackers(taskId: number) {
    const list = await getAllTimeTrackers(taskId, selectedCollaborator.value || undefined)
    timeTrackers.value[taskId] = list
    const open = list.find(tt => tt.startDate === tt.endDate)
    if (open) activeTrackerByTask.value[taskId] = open.id
    else delete activeTrackerByTask.value[taskId]
}

async function refreshTotals() {
    try {
        const collId = selectedCollaborator.value!
        todayTotal.value = await getTodayTotal(collId)
        monthTotal.value = await getMonthTotal(collId)
    } catch {
        todayTotal.value = '00:00'
        monthTotal.value = '00:00'
    }
}

watch(selectedCollaborator, () => loadTasks())

let pendingTask: ITask | null = null
function openDialog(task: ITask) {
    pendingTask = task
    form.collaboratorId = null
    valid.value = false
    dialog.value = true
}

async function confirmStart() {
    if (!pendingTask || form.collaboratorId == null) return
    try {
        const now = new Date().toISOString()
        const dto: ITimeTrackerCreate = {
            taskId: pendingTask.id,
            collaboratorId: form.collaboratorId,
            startDate: now,
            endDate: now,
            timeZoneId: Intl.DateTimeFormat().resolvedOptions().timeZone
        }
        const created = await createTimeTracker(dto)
        activeTrackerByTask.value[pendingTask.id] = created.id
        await loadTimeTrackers(pendingTask.id)
        await refreshTotals()
        dialog.value = false
    } catch (e: any) {
        snackbar.value = {
            show: true,
            message: e.response?.data?.title || e.response?.data || e.message,
            color: 'error'
        }
    }
}

async function stop(task: ITask) {
    const trackerId = activeTrackerByTask.value[task.id]
    if (!trackerId) return
    try {
        const list = timeTrackers.value[task.id] || []
        const open = list.find(x => x.id === trackerId)
        if (!open) throw new Error("Tracker não encontrado")

        const start = new Date(open.startDate)
        const now = new Date()
        const maxBy24h = new Date(start.getTime() + 24 * 3600 * 1000)
        const futuros = list
            .filter(x => new Date(x.startDate) > start && x.id !== trackerId)
            .map(x => new Date(x.startDate))
        const nextStart = futuros.length
            ? new Date(Math.min(...futuros.map(d => d.getTime())))
            : null

        let effectiveEnd = now
        if (effectiveEnd > maxBy24h) effectiveEnd = maxBy24h
        if (nextStart && effectiveEnd > nextStart) effectiveEnd = nextStart

        await updateTimeTracker(trackerId, {
            startDate: open.startDate,
            endDate: effectiveEnd.toISOString()
        })

        delete activeTrackerByTask.value[task.id]
        await loadTimeTrackers(task.id)
        await refreshTotals()
    } catch (e: any) {
        snackbar.value = {
            show: true,
            message: e.response?.data?.title || e.response?.data || e.message,
            color: 'error'
        }
        delete activeTrackerByTask.value[task.id]
        await loadTimeTrackers(task.id)
        await refreshTotals()
    }
}

async function onDeleteTask(taskId: number) {
    try {
        await deleteTask(taskId)
        await loadTasks()
    } catch (e: any) {
        snackbar.value = {
            show: true,
            message: e.response?.data || e.message,
            color: 'error'
        }
    }
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