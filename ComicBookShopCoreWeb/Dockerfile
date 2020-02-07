FROM node:12.2.0 AS builder

WORKDIR /webApp

COPY package.json package.json
RUN npm install
RUN npm install -g @angular/cli
COPY . .

CMD ng serve --host 0.0.0.0