import React, { useState } from 'react';
import { Alert, Button, StyleSheet, Text, TextInput, View } from 'react-native';
import { useAuth } from '../../hooks/useAuth';

export default function LoginScreen() {
  const { login } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async () => {
    setIsLoading(true);
    const result = await login(email, password);
    setIsLoading(false);
    if (result) {
      Alert.alert("Error", result.message);
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Simon Movilidad</Text>
      <TextInput
        style={styles.input}
        placeholder="Correo electrónico"
        value={email}
        onChangeText={setEmail}
        autoCapitalize="none"
        keyboardType="email-address"
        placeholderTextColor="#888"
      />
      <TextInput
        style={styles.input}
        placeholder="Contraseña"
        value={password}
        onChangeText={setPassword}
        secureTextEntry
        placeholderTextColor="#888"
      />
      <Button 
        title={isLoading ? "Iniciando..." : "Iniciar Sesión"} 
        onPress={handleLogin} 
        disabled={isLoading}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: 'center', padding: 20, backgroundColor: '#1a1d21' },
  title: { fontSize: 32, fontWeight: 'bold', textAlign: 'center', marginBottom: 40, color: '#e0e0e0' },
  input: { height: 50, borderColor: '#4a4a4a', borderWidth: 1, paddingHorizontal: 15, borderRadius: 8, marginBottom: 15, color: '#e0e0e0'},
});