import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";

const AuthModule = ({ onAuthSuccess }) => {
    const [isLogin, setIsLogin] = useState(true);
    const [formData, setFormData] = useState({
        name: "",
        email: "",
        password: "",
        confirmPassword: "",
    });

    const toggleForm = () => {
        setIsLogin(!isLogin);
        setFormData({ name: "", email: "", password: "", confirmPassword: "" });
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!formData.email || !formData.password || (!isLogin && !formData.name)) {
            alert("Please fill all required fields!");
            return;
        }
        if (!isLogin && formData.password !== formData.confirmPassword) {
            alert("Passwords do not match!");
            return;
        }

        // Dummy authentication success
        if (isLogin) {
            alert(`Logged in with Email: ${formData.email}`);
        } else {
            alert(`Signed up with Name: ${formData.name}, Email: ${formData.email}`);
        }

        // Call parent to update authentication state
        onAuthSuccess();
    };

    return (
        <div className="container d-flex justify-content-center align-items-center vh-100">
            <div className="card p-4 shadow" style={{ maxWidth: "400px", width: "100%" }}>
                <h3 className="text-center mb-4">{isLogin ? "Login" : "Sign Up"}</h3>
                <form onSubmit={handleSubmit}>
                    {!isLogin && (
                        <div className="mb-3">
                            <label htmlFor="name" className="form-label">Name</label>
                            <input
                                type="text"
                                className="form-control"
                                id="name"
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                            />
                        </div>
                    )}
                    <div className="mb-3">
                        <label htmlFor="email" className="form-label">Email</label>
                        <input
                            type="email"
                            className="form-control"
                            id="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="password" className="form-label">Password</label>
                        <input
                            type="password"
                            className="form-control"
                            id="password"
                            name="password"
                            value={formData.password}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    {!isLogin && (
                        <div className="mb-3">
                            <label htmlFor="confirmPassword" className="form-label">Confirm Password</label>
                            <input
                                type="password"
                                className="form-control"
                                id="confirmPassword"
                                name="confirmPassword"
                                value={formData.confirmPassword}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    )}
                    <button type="submit" className="btn btn-primary w-100">
                        {isLogin ? "Login" : "Sign Up"}
                    </button>
                </form>
                <div className="text-center mt-3">
                    <span>
                        {isLogin ? "Don't have an account?" : "Already have an account?"}{" "}
                        <button className="btn btn-link p-0" onClick={toggleForm}>
                            {isLogin ? "Sign Up" : "Login"}
                        </button>
                    </span>
                </div>
            </div>
        </div>
    );
};

export default AuthModule;
