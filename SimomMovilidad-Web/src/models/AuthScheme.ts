import z from "zod";

export const AuthScheme = z.object({
  email: z
    .email("Debe ser un correo electrónico válido")
    .min(1, "El correo electrónico es obligatorio"),
  password: z.string()
    .min(6, "La contraseña debe tener al menos 6 carácteres")
});

export type LoginForm = z.infer<typeof AuthScheme>;