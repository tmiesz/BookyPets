import { useState } from "react";
import { useForm } from "react-hook-form";
import { Link, useLocation } from "react-router-dom";
import ErrorMessage from "../components/ErrorMessage";
import { useAuth } from "../context/useAuth";
import { toApiError, type ApiError } from "../types/ApiError";
import "../styles/Auth.css"

interface LoginFormData {
    email: string,
    password: string
}

export default function Login() {
    const { login, sessionExpired } = useAuth();
    const location = useLocation();
    const [serverError, setServerError] = useState<ApiError | null>(null);

    const { register, handleSubmit, formState: { errors, isSubmitting } }
        = useForm<LoginFormData>();

    async function onSubmit(data: LoginFormData) {
        setServerError(null);

        try {
            await login(data.email, data.password);
        } catch (error) {
            setServerError(toApiError(error));
        }
    }

    return (

        <div className="auth">
            <div className="auth__container">
                <div className="auth__content">
                    <h1 className="auth__title">Login</h1>
                    <form className="auth__form" onSubmit={handleSubmit(onSubmit)}>

                        {sessionExpired && (
                            <div role="status" className="auth__notice">
                                Your session has expired. Please log in again.
                            </div>
                        )}

                        <ErrorMessage error={serverError} className="auth__error" />

                        <div className="auth__group">
                            <label className="auth__label" htmlFor="email">Email
                                <input className="auth__input"
                                    id="email"
                                    type="email"
                                    autoComplete="email"
                                    aria-invalid={errors.email ? "true" : "false"}
                                    aria-describedby={errors.email ? "email-error" : undefined}
                                    {...register('email', { required: "Email is required" })}
                                />
                            </label>
                            {errors.email && <span id="email-error" role="alert" className="auth__error">{errors.email.message}</span>}
                        </div>

                        <div className="auth__group">
                            <label className="auth__label" htmlFor="password">Password
                                <input className="auth__input"
                                    id="password"
                                    type="password"
                                    autoComplete="current-password"
                                    aria-invalid={errors.password ? "true" : "false"}
                                    aria-describedby={errors.password ? "password-error" : undefined}
                                    {...register('password', { required: "Password is required" })}
                                />
                            </label>
                            {errors.password && <span id="password-error" role="alert" className="auth__error">{errors.password.message}</span>}
                        </div>

                        <button className="btn auth__button auth__button--secondary" type="submit" disabled={isSubmitting}>
                            {isSubmitting ? "Logging in..." : "Login"}
                        </button>
                    </form>

                    <div className="auth__switch">
                        <p>Don't have an account? <Link className="btn auth__link auth__link--primary" to="/register" state={location.state}>Sign up</Link></p>
                    </div>
                </div>
            </div>

        </div>
    )
}
