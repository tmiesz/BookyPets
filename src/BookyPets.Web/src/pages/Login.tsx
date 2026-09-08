import { useContext } from "react";
import { useForm } from "react-hook-form";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import "../styles/Login.css"

interface AuthFormData {
    firstname: string,
    lastname: string,
    email: string,
    password: string
}

export default function Login() {
    const authContext = useContext(AuthContext);
    if (!authContext) throw new Error("Auth must be used within AuthProvider");
    const { login, error } = authContext;

    const navigate = useNavigate()

    const { register, handleSubmit, formState: { errors } }
        = useForm<AuthFormData>();

    async function onSubmit(data: AuthFormData) {
        const success =  await login(data.email, data.password);
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
                                    {...register('password', {
                                        required: "Password is required",
                                        minLength: { value: 8, message: "Password must be at least 8 characters" },
                                        maxLength: { value: 100, message: "Password must be less than 100 characters" },
                                        validate: {
                                            uppercase: value =>
                                                (value.match(/[A-Z]/g) || []).length >= 2 ||
                                                "Password must contain atleast 2 uppercase letters.",
                                            lowercase: value =>
                                                (value.match(/[a-z]/g) || []).length >= 3 ||
                                                "Password must contain atleast 3 lowercase letters.",
                                            numbers: value =>
                                                (value.match(/[0-9]/g) || []).length >= 2 ||
                                                "Password must contain atleast 2 numbers.",
                                            special: value =>
                                                /[!@#$&*]/.test(value) ||
                                                "Password must contain atleast 2 uppercase letters."
                                        }
                                    })}
                                />
                            </label>
                            {errors.password && <span className="form-error">{errors.password.message}</span>}
                        </div>

                        <button className="btn btn-primary btn-large" type="submit">
                            Sign Up
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
