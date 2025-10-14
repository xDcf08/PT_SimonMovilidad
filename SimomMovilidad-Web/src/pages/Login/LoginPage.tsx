import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm, type SubmitHandler } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { InputForm } from "../../components/CustomInput/InputForm";
import { useAuth } from "../../hooks/useAuth";
import { AuthScheme, type LoginForm } from "../../models/AuthScheme";
import './LoginPage.css'


export const LoginPage = () =>  {

  const navigate = useNavigate();
  const { login } = useAuth();

  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);

  const { control, handleSubmit, formState: { errors } } = useForm<LoginForm>({
    resolver: zodResolver(AuthScheme),
    defaultValues: {
      email: "",
      password: ""
    }
  });

  const onSubmit : SubmitHandler<LoginForm> = async (data) => {
    setIsLoading(true);
    setApiError(null);

    const result = await login(data.email, data.password);

    console.log(result)

    setIsLoading(false)

    if(result.success){
      navigate('/dashboard')
    }else{
      setApiError(result.message || "Ocurrió un error inesperado");
    }
  }

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleSubmit(onSubmit)}>
        <h2>Iniciar sesión</h2>
        <p>Bienvenido de nuevo</p>

        {apiError && <p className="api-error-message">{apiError}</p>}

        <InputForm name="email" control={control} type="email" label="Correo electrónico" error={errors.email}/>
        <InputForm name="password" control={control} type="password" label="Contraseña" error={errors.password}/>
        <button type="submit" className="login-button">
          {isLoading ? 'Iniciando sesión...' : 'Iniciar sesión'}
        </button>
      </form>
    </div>
  )
}

