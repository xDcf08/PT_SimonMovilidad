import { Stack, useRouter } from 'expo-router';
import 'react-native-reanimated';

import { useAuth } from '@/hooks/useAuth';
import { useHydration } from '@/hooks/useHydration';
import { useEffect } from 'react';
import { View } from 'react-native';

function Splash() {
  return <View style={{ flex: 1, backgroundColor: '#1a1d21' }} />;
}

export default function RootLayout() {

  const { isAuthenticated } = useAuth();
  const hasHydrated = useHydration();
  const router = useRouter();

  useEffect(() => {

    console.log(hasHydrated, "hasHydrated")

    if (!hasHydrated) {
      return;
    }
    console.log(isAuthenticated)

    if (isAuthenticated) {
      router.replace('/(app)/dashboard');
    } else if(!isAuthenticated ) {
      router.replace('/(auth)/login');
    }
  }, [isAuthenticated, hasHydrated]);


  return (
    <Stack screenOptions={{ headerShown: false }}>
      <Stack.Screen name="(auth)" />
      <Stack.Screen name="(app)" />
    </Stack>
  );
}
