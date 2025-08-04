# syntax=docker/dockerfile:1.12-labs

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY --exclude=nginx.conf . .
RUN dotnet publish "QuizApp.UI/QuizApp.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM nginx:alpine
RUN apk update && apk upgrade

RUN chown -R nginx:nginx /usr/share/nginx/html && chmod -R 755 /usr/share/nginx/html && \
       chown -R nginx:nginx /var/cache/nginx && \
       chown -R nginx:nginx /var/log/nginx && \
       chown -R nginx:nginx /etc/nginx/conf.d
RUN touch /var/run/nginx.pid && \
       chown -R nginx:nginx /var/run/nginx.pid


USER nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Expose server port from nginx.conf
EXPOSE 8888

CMD ["nginx", "-g", "daemon off;"]
