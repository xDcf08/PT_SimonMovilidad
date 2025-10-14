import { useEffect, useState } from 'react';
import { useAuthStore } from '../stores/authStore';

export const useHydration = () => {
  const [hydrated, setHydrated] = useState(false);

  useEffect(() => {
    // La función hasHydrated() nos dice si el store ya terminó de cargar desde AsyncStorage.
    const unsubHydrate = useAuthStore.persist.onHydrate(() => setHydrated(false));
    const unsubFinishHydration = useAuthStore.persist.onFinishHydration(() => setHydrated(true));

    setHydrated(useAuthStore.persist.hasHydrated());

    return () => {
      unsubHydrate();
      unsubFinishHydration();
    };
  }, []);

  return hydrated;
};