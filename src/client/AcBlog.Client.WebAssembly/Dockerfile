FROM nginx:alpine
COPY ./src/client/AcBlog.Client.WebAssembly/docker/nginx.conf /etc/nginx/nginx.conf
COPY ./src/client/AcBlog.Client.WebAssembly/docker/mime.types /etc/nginx/mime.types
COPY ./src/client/AcBlog.Client.WebAssembly/publish/wwwroot /app/

VOLUME /app/data
EXPOSE 80
