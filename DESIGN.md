---
project: OsakidetzaListas
version: 1.0
source: Stitch-style design system file
colors:
  background: "#f8f9fa"
  surface: "#ffffff"
  surface-alt: "#f0f2f5"
  sidebar-start: "#052767"
  sidebar-end: "#3a0647"
  primary: "#0d6efd"
  info: "#0dcaf0"
  success: "#198754"
  warning: "#ffc107"
  danger: "#dc3545"
  text: "#212529"
  muted: "#6c757d"
typography:
  font-family: "Helvetica Neue, Helvetica, Arial, sans-serif"
  base-size: "16px"
  line-height: "1.5"
radius:
  sm: "6px"
  md: "10px"
  lg: "14px"
spacing:
  xs: "0.25rem"
  sm: "0.5rem"
  md: "1rem"
  lg: "1.5rem"
  xl: "2rem"
shadow:
  card: "0 8px 24px rgba(0, 0, 0, 0.08)"
  hover: "0 10px 28px rgba(0, 0, 0, 0.12)"
---

# Overview

OsakidetzaListas is a data-heavy administrative dashboard for inspecting list rankings, searching people, and simulating opposition outcomes. The UI should feel reliable, dense, and easy to scan, with clear hierarchy and minimal decorative noise.

## Visual Direction

- Functional first, not flashy.
- Blue-led sidebar with a restrained accent gradient.
- White cards on a light neutral canvas.
- Tables and filters should remain the center of attention.

## Color System

- Use `primary` for actions, links, and highlighted totals.
- Use `success` and `danger` for availability states.
- Use `warning` for cutoff points and attention states.
- Keep background areas light and neutral.
- Avoid saturated decorative colors outside status indicators.

## Typography

- Use `Helvetica Neue, Helvetica, Arial, sans-serif`.
- Headings should be compact and bold.
- Body text should stay readable at small sizes because most screens are information dense.
- Numbers in tables should align visually and remain easy to compare.

## Layout

- Keep the left sidebar fixed on desktop and stacked on mobile.
- Use card-based sections to group filters, summaries, and data tables.
- Prefer wide tables with sticky-feeling hierarchy over fragmented mini widgets.
- Preserve responsive behavior for mobile without hiding important controls.

## Components

- Cards: white background, subtle shadow, rounded corners.
- Tables: compact rows, strong headers, striped rows only when useful.
- Sidebar: dark gradient, white brand text, clear active state.
- Buttons: Bootstrap-based, with color meaning tied to the action.
- Modals: centered, scrollable, data-friendly, not decorative.

## Interaction Rules

- The user should always know what selection is driving the current result.
- Filters should reset logically when upstream inputs change.
- Loading states should be explicit and brief.
- Empty states should explain the next action.

## Do

- Keep the interface fast to scan.
- Use badges and muted helper text to explain state.
- Keep status colors consistent across pages.
- Favor readable tables over custom visual gimmicks.

## Don’t

- Don’t introduce unnecessary motion.
- Don’t replace the current admin/dashboard feel with a consumer-style marketing layout.
- Don’t hide critical actions behind nested menus.
- Don’t use multiple competing accent palettes.

