import { useEffect, useId, useRef, type ReactNode } from "react";
import "../styles/Picker.css";

interface PickerDialogProps {
    title: string,
    onClose: () => void,
    children: ReactNode
}

export default function PickerDialog({ title, onClose, children }: PickerDialogProps) {
    const dialogRef = useRef<HTMLDialogElement>(null);
    const previouslyFocused = useRef<Element | null>(document.activeElement);
    const titleId = useId();

    useEffect(() => {
        const dialog = dialogRef.current;
        const focusTarget = previouslyFocused.current;

        if (!dialog) return;

        if (!dialog.open) dialog.showModal();

        const closeOnBackdropClick = (event: MouseEvent) => {
            if (event.target === dialog) dialog.close();
        };
        dialog.addEventListener("click", closeOnBackdropClick);

        return () => {
            dialog.removeEventListener("click", closeOnBackdropClick);
            if (focusTarget instanceof HTMLElement) focusTarget.focus();
        };
    }, []);

    return (
        <dialog ref={dialogRef} className="picker" aria-labelledby={titleId} onClose={onClose}>
            <div className="picker__dialog">
                <div className="picker__header">
                    <h2 className="picker__title" id={titleId}>{title}</h2>
                    <button type="button" className="btn picker__close" onClick={() => dialogRef.current?.close()}>Close</button>
                </div>

                {children}
            </div>
        </dialog>
    );
}
