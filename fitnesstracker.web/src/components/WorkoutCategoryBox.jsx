import '../styles/WorkoutCategoryBox.css'

export default function WorkoutCategoryBox({ category, isSelected, onSelect }) {
  const handleClick = () => {
    onSelect(category)
  }

  return (
    <div
      className={`category-box ${isSelected ? 'selected' : ''}`}
      onClick={handleClick}
    >
      <span className="category-name">{category}</span>
      {isSelected && <span className="checkmark">✓</span>}
    </div>
  )
}
