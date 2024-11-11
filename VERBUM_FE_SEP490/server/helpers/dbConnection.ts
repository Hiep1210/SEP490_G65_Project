// server/helpers/dbConnection.ts
import pg from 'pg';

const { Client } = pg;

// Replace this with your actual PostgreSQL connection string
const connectionString = 'postgresql://verbum-verbum.h.aivencloud.com:17672/verbum?user=avnadmin&password=AVNS_3gU17QnAszLI3He-LgO';

// Create a new PostgreSQL client instance with the SSL configuration
const client = new Client({
  connectionString,
  ssl: {
    rejectUnauthorized: false, // Allows self-signed certificates
  },
});

client.connect()
  .then(() => console.log('Connected to the PostgreSQL database'))
  .catch((err) => console.error('Error connecting to the database:', err));

export default client;
