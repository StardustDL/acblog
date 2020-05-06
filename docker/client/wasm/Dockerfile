FROM nginx:alpine
COPY ./docker/client/wasm/nginx.conf /etc/nginx/nginx.conf
COPY ./docker/client/wasm/mime.types /etc/nginx/mime.types
COPY ./src/AcBlog.Client.WASM/publish/wwwroot /app/

VOLUME /app/data
EXPOSE 80
