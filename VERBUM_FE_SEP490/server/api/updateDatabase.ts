import type { IncomingMessage, ServerResponse } from 'http';
import client from '../helpers/dbConnection';

export default async (req: IncomingMessage, res: ServerResponse) => {
  // Parse body from incoming request
  const buffers: Uint8Array[] = [];
  for await (const chunk of req) buffers.push(chunk);
  const body = JSON.parse(Buffer.concat(buffers).toString());

  const { clientId, transactionId, orderId, isDeposit } = body;

  const query = `
    UPDATE client_transaction
    SET orderId = $1, isDeposit = $2
    WHERE clientId = $3 AND transactionId = $4
  `;
  const values = [orderId, isDeposit === 'true', clientId, transactionId];

  try {
    const result = await client.query(query, values);
    res.statusCode = result.rowCount && result.rowCount > 0 ? 200 : 404;
    res.end(
      JSON.stringify({
        message: result.rowCount && result.rowCount > 0 
          ? 'Payment confirmed and database updated.' 
          : 'No matching records found.'
      })
    );
  } catch (error) {
    console.error('Database update error:', error);
    res.statusCode = 500;
    res.end(JSON.stringify({ message: 'Error updating payment.', error }));
  }
};
