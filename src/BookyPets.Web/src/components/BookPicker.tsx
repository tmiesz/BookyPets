import "../styles/Picker.css";

type Props = {
    onClose: () => void;
};

export default function BookPicker({ onClose }: Props) {
    return (
        <div className="picker-overlay">
            <div className="picker">
                <button onClick={onClose}>Close</button>
                <p>Pick a book</p>
            </div>
        </div>
    );
}
