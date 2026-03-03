import { createRouter, createMemoryHistory } from 'vue-router'

const routes = [
    {
    path: '/',
    name: 'property',
    component: () => import('../layouts/Default.vue'),
    children: [
      {
        path: '',
        name: 'propertyMain',
        component: () => import('../views/PropertyView.vue')
      }
    ]
  },
  {
    path: '/room',
    name: 'room',
    component: () => import('../layouts/Default.vue'),
    children: [
      {
        path: '',
        name: 'roomMain',
        component: () => import('../views/RoomView.vue')
      }
    ]
  },
]

const router = createRouter({
  history: createMemoryHistory(),
  routes,
});

export default router
