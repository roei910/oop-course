# Step 1: Use Node.js to build the Angular app
FROM node:18 AS build

WORKDIR /app
# COPY package*.json ./
COPY . .
# RUN rm package-lock.json
RUN npm install
RUN npm run build --configuration=development
# RUN npm run build --configuration=${ANGULAR_BUILD_CONFIGURATION}

# Step 2: Use Nginx to serve the built Angular app
FROM nginx:1.23-alpine

# Copy the built app from the previous stage
COPY --from=build /app/dist/client-angular /usr/share/nginx/html

# Copy a custom Nginx config (optional)
# COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]