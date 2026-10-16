import type { Pet } from "../types/Pet";
import PetCard from "./PetCard";
import PickerDialog from "./PickerDialog";

type Props = {
    pets: Pet[];
    onClose: () => void;
    onSelect: (pet: Pet) => void;
};

export default function PetPicker({ pets, onClose, onSelect }: Props) {
    const handleSelect = (pet: Pet) => {
        onSelect(pet);
        onClose();
    }

    return (
        <PickerDialog title="Pick a pet" onClose={onClose}>
            {pets.length === 0 ? (
                <p className="picker__empty">You don't have any pets yet. Acquire one on the Pets page first.</p>
            ) : (
                <ul className="picker__list">
                    {pets.map((pet) => (
                        <li key={pet.id} className="picker__item">
                            <PetCard pet={pet} />
                            <button
                                type="button"
                                className="picker__option"
                                aria-label={`Select ${pet.name}`}
                                onClick={() => handleSelect(pet)} />
                        </li>
                    ))}
                </ul>
            )}
        </PickerDialog>
    );
}
