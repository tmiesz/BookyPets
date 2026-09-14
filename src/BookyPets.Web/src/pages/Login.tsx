import { useForm } from "react-hook-form";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import "../styles/Login.css"

interface AuthFormData {
    email: string,
    password: string
}

export default function Login() {
    const { login, error } = useAuth();

    const navigate = useNavigate()

    const { register, handleSubmit, formState: { errors } }
        = useForm<AuthFormData>();

    async function onSubmit(data: AuthFormData) {
        const success = await login(data.email, data.password);
        if (success) navigate("/")
    }

    return (

        <div className="page">
            <div className="container">
                <div className="auth-container">
                    <h1 className="page-title">Login</h1>
                    <form className="auth-form" onSubmit={handleSubmit(onSubmit)}>

                        {error && <div className="error-message">{error.detail}</div>}

                        <div className="form-group">
                            <label className="form-label" htmlFor="email">Email
                                <input className="form-input"
                                    id="email"
                                    type="email"
                                    {...register('email', { required: "Email is required" })}
                                />
                            </label>
                            {errors.email && <span className="form-error">{errors.email.message}</span>}
                        </div>

                        <div className="form-group">
                            <label className="form-label" htmlFor="password">Password
                                <input className="form-input"
                                    id="password"
                                    type="password"
                                    {...register('password', { required: "Password is required" })}
                                />
                            </label>
                            {errors.password && <span className="form-error">{errors.password.message}</span>}
                        </div>

                        <button className="btn btn-primary btn-large" type="submit">
                            Login
                        </button>
                    </form>

                    <div className="auth-switch">
                        <p>Dont have an account?<span className="auth-link">Sign up</span></p>
                    </div>
                </div>
            </div>

        </div>
    )
}
