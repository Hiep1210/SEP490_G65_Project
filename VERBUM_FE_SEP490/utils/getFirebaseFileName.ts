import { getStorage, ref } from 'firebase/storage';

export const getFirebaseFileName = (downloadURL: string) => {
  const storage = getStorage();
  const httpsReference = ref(storage, downloadURL);

  return httpsReference.name;

}

export const getJobName = (name: string) => {
  const parts = name.split('_');
  if (parts.length < 4) return name; // Return the whole string if less than 3 underscores
  const prefix = parts.slice(0, 3).join('_')
  const jobName = getFirebaseFileName(parts.slice(3).join('_'));
  return prefix.concat('_',jobName);
}