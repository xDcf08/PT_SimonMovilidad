import { Controller, type Control, type FieldError } from "react-hook-form"
import type { LoginForm } from "../../models/AuthScheme"

interface Props{
  name: keyof LoginForm,
  control: Control<LoginForm>,
  label: string,
  type?: string,
  error: FieldError | undefined
}

export const InputForm = ({name, control, label, type, error} : Props) => {
  return (
    <div className="form-group">
      <label htmlFor={name}>{label}</label>
      <Controller
        name={name}
        control={control}
        render={({field}) => 
          <input 
            id={name}
            type={type ?? "text"}
            {...field}
            className={`form-control ${error ? 'is-invalid' : ''}`}
            placeholder={label}
          />
        }
      />
      {error && <p className="error-message">{error.message}</p>}
    </div>
  )
}