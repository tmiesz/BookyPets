import { useState } from "react";
import { useForm } from "react-hook-form";
import { Link, useLocation } from "react-router-dom";
import ErrorMessage from "../components/ErrorMessage";
import { useAuth } from "../context/useAuth";
import { toApiError, type ApiError } from "../types/ApiError";
import "../styles/Auth.css"

interface RegisterFormData {
    firstname: string,
    lastname: string,
    email: string,
    password: string
}

export default function Register() {
    const { signUp } = useAuth();
    const location = useLocation();
    const [serverError, setServerError] = useState<ApiError | null>(null);

    const { register, handleSubmit, formState: { errors, isSubmitting } }
        = useForm<RegisterFormData>();

    async function onSubmit(data: RegisterFormData) {
        setServerError(null);

        try {
            await signUp(data.firstname, data.lastname, data.email, data.password);
        } catch (error) {
            setServerError(toApiError(error));
        }
    }

    return (

        <div className="auth">
            <div className="auth__container">
                <div className="auth__content">
                    <h1 className="auth__title">Sign Up</h1>
                    <form className="auth__form" onSubmit={handleSubmit(onSubmit)}>

                        <ErrorMessage error={serverError} className="auth__error" />

                        <div className="auth__group">
                            <label className="auth__label" htmlFor="firstname">First Name
                                <input className="auth__input"
                                    id="firstname"
                                    type="text"
                                    aria-invalid={errors.firstname ? "true" : "false"}
                                    aria-describedby={errors.firstname ? "firstname-error" : undefined}
                                    {...register('firstname', { required: "First name is required" })}
                                />
                            </label>
                            {errors.firstname && <span id="firstname-error" role="alert" className="auth__error">{errors.firstname.message}</span>}
                        </div>

                        <div className="auth__group">
                            <label className="auth__label" htmlFor="lastname">Last Name
                                <input className="auth__input"
                                    id="lastname"
                                    type="text"
                                    aria-invalid={errors.lastname ? "true" : "false"}
                                    aria-describedby={errors.lastname ? "lastname-error" : undefined}
                                    {...register('lastname', { required: "Last name is required" })}
                                />
                            </label>
                            {errors.lastname && <span id="lastname-error" role="alert" className="auth__error">{errors.lastname.message}</span>}
                        </div>

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
                                    autoComplete="new-password"
                                    aria-invalid={errors.password ? "true" : "false"}
                                    aria-describedby={errors.password ? "password-error" : undefined}
                                    {...register('password', {
                                        required: "Password is required",
                                        minLength: { value: 8, message: "Password must be at least 8 characters" },
                                        maxLength: { value: 128, message: "Password must be at most 128 characters" },
                                        validate: {
                                            uppercase: value =>
                                                (value.match(/[A-Z]/g) || []).length >= 2 ||
                                                "Password must contain at least 2 uppercase letters.",
                                            lowercase: value =>
                                                (value.match(/[a-z]/g) || []).length >= 3 ||
                                                "Password must contain at least 3 lowercase letters.",
                                            numbers: value =>
                                                (value.match(/[0-9]/g) || []).length >= 2 ||
                                                "Password must contain at least 2 numbers.",
                                            special: value =>
                                                /[!@#$&*]/.test(value) ||
                                                "Password must contain at least 1 special character (! @ # $ & *)."
                                        }
                                    })}
                                />
                            </label>
                            {errors.password && <span id="password-error" role="alert" className="auth__error">{errors.password.message}</span>}
                        </div>

                        <button className="btn auth__button auth__button--primary" type="submit" disabled={isSubmitting}>
                            {isSubmitting ? "Signing up..." : "Sign Up"}
                        </button>
                    </form>

                    <div className="auth__switch">
                        <p>Already have an account? <Link className="btn auth__link auth__link--secondary" to="/login" state={location.state}>Login</Link></p>
                    </div>
                </div>
            </div>

        </div >
    )
}
