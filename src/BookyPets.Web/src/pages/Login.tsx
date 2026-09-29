import { useForm } from "react-hook-form";
import { useAuth } from "../context/AuthContext";
import { Link, useNavigate } from "react-router-dom";
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

        <div className="login">
            <div className="login__container">
                <div className="login__content">
                    <h1 className="login__title">Login</h1>
                    <form className="login__form" onSubmit={handleSubmit(onSubmit)}>

                        {error && <div className="login__error">{error.detail}</div>}

                        <div className="login__group">
                            <label className="login__label" htmlFor="email">Email
                                <input className="login__input"
                                    id="email"
                                    type="email"
                                    {...register('email', { required: "Email is required" })}
                                />
                            </label>
                            {errors.email && <span className="login__error">{errors.email.message}</span>}
                        </div>

                        <div className="login__group">
                            <label className="login__label" htmlFor="password">Password
                                <input className="login__input"
                                    id="password"
                                    type="password"
                                    {...register('password', { required: "Password is required" })}
                                />
                            </label>
                            {errors.password && <span className="login__error">{errors.password.message}</span>}
                        </div>

                        <button className="btn login__button" type="submit">
                            Login
                        </button>
                    </form>

                    <div className="login__switch">
                        <p>Don't have an account? <Link className="btn login__link" to="/register">Sign up</Link></p>
                    </div>
                </div>
            </div>

        </div>
    )
}
