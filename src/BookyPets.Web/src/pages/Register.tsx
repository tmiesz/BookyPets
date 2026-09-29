import { useForm } from "react-hook-form";
import { useAuth } from "../context/AuthContext";
import { Link, useNavigate } from "react-router-dom";
import "../styles/Register.css"

interface AuthFormData {
    firstname: string,
    lastname: string,
    email: string,
    password: string
}

export default function Register() {
    const { signUp, error } = useAuth();

    const navigate = useNavigate()

    const { register, handleSubmit, formState: { errors } }
        = useForm<AuthFormData>();

    async function onSubmit(data: AuthFormData) {
        const success = await signUp(data.firstname, data.lastname, data.email, data.password);

        if (success) navigate("/")
    }

    return (

        <div className="register">
            <div className="register__container">
                <div className="register__content">
                    <h1 className="register__title">Sign Up</h1>
                    <form className="register__form" onSubmit={handleSubmit(onSubmit)}>

                        {error && <div className="register_error">{error.detail}</div>}

                        <div className="register_group">
                            <label className="register__label" htmlFor="firstname">First Name
                                <input className="register__input"
                                    id="firstname"
                                    type="text"
                                    {...register('firstname', { required: "First name is required" })}
                                />
                            </label>
                            {errors.firstname && <span className="register__error">{errors.firstname.message}</span>}
                        </div>

                        <div className="register__group">
                            <label className="register__label" htmlFor="lastname">Last Name
                                <input className="register__input"
                                    id="lastname"
                                    type="text"
                                    {...register('lastname', { required: "Last name is required" })}
                                />
                            </label>
                            {errors.lastname && <span className="register__error">{errors.lastname.message}</span>}
                        </div>

                        <div className="register__group">
                            <label className="register__label" htmlFor="email">Email
                                <input className="register__input"
                                    id="email"
                                    type="email"
                                    {...register('email', { required: "Email is required" })}
                                />
                            </label>
                            {errors.email && <span className="register__error">{errors.email.message}</span>}
                        </div>

                        <div className="register__group">
                            <label className="register__label" htmlFor="password">Password
                                <input className="register__input"
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
                            {errors.password && <span className="register__error">{errors.password.message}</span>}
                        </div>

                        <button className="btn register__button" type="submit">
                            Sign Up
                        </button>
                    </form>

                    <div className="register__switch">
                        <p>Already have an account? <Link className="btn register__link" to="/login"> Login</Link></p>
                    </div>
                </div>
            </div>

        </div >
    )
}
