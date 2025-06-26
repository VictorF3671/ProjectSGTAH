# etapa de build
FROM node:18-alpine AS build

WORKDIR /app

# copia package.json + package-lock e instala dependências
COPY package*.json ./
RUN npm ci

# copia todo o código e gera a build de produção
COPY . .
RUN npm run build

# etapa de runtime
FROM nginx:1.25-alpine

# remove configuração default
RUN rm /etc/nginx/conf.d/default.conf

# copia nossa config mínima de nginx
COPY ./nginx.conf /etc/nginx/conf.d/app.conf

# copia build do estágio anterior
COPY --from=build /app/dist /usr/share/nginx/html

# expõe porta
EXPOSE 80

# comando default
CMD ["nginx", "-g", "daemon off;"]