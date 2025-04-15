import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "challengetiles.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

//determine the API base URL
const API_BASE_URL = env.NODE_ENV === "production"
    //? "http://challengetilesbackend-env.eba-9e2vzyts.us-east-2.elasticbeanstalk.com/api" //elastic beanstalk url
    ? "https://r93ormq7xf.execute-api.us-east-2.amazonaws.com/dev/api" //api gateway stage invoke url
    : "https://localhost:7055/api"; //use local server in dev

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    define: {
        'import.meta.env.VITE_API_URL': JSON.stringify(API_BASE_URL) //inject API URL
    },
    server: {
        proxy: {
            '^/weatherforecast': {
                target: API_BASE_URL,
                secure: false
            }
        },
        port: 63304,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
