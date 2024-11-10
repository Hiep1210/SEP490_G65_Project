import { getStorage, ref } from 'firebase/storage';

export const getFirebaseFileName = (downloadURL: string) => {
  const storage = getStorage();
  const httpsReference = ref(storage, downloadURL);

  return httpsReference.name;

}