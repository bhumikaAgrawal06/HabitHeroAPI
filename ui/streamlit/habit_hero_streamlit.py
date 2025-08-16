
import os
import time
import requests
import streamlit as st

# --- Config ---
DEFAULT_API_BASE = os.getenv("HABIT_HERO_API", "http://localhost:5000")

st.set_page_config(page_title="Habit Hero (Streamlit)", page_icon="🦸", layout="wide")
st.title("🦸 Habit Hero — Streamlit UI")
st.caption("Simple UI that talks to your ASP.NET Core API")

# --- Sidebar settings ---
st.sidebar.header("Settings")
api_base = st.sidebar.text_input("API Base URL", value=DEFAULT_API_BASE, help="Example: http://localhost:5000 or https://localhost:7200")
if st.sidebar.button("Reload"):
    st.experimental_rerun()

# Session state for minor notifications
if "last_action" not in st.session_state:
    st.session_state.last_action = ""

# --- Helpers ---
def api_get(path: str):
    r = requests.get(f"{api_base}{path}", timeout=10)
    r.raise_for_status()
    return r.json()

def api_post(path: str, json=None):
    r = requests.post(f"{api_base}{path}", json=json, timeout=10)
    r.raise_for_status()
    # created or ok
    if r.status_code in (200, 201):
        return r.json()
    return None

def api_put(path: str, json=None):
    r = requests.put(f"{api_base}{path}", json=json, timeout=10)
    r.raise_for_status()
    return r.json()

def api_delete(path: str):
    r = requests.delete(f"{api_base}{path}", timeout=10)
    if r.status_code not in (200, 204):
        r.raise_for_status()

def human_time(ts: str):
    try:
        # Simple pretty; Streamlit can format string directly
        return ts.replace("T", " ").replace("Z", " UTC")
    except Exception:
        return str(ts)

# --- Create form ---
st.subheader("Create a Habit")
with st.form("create_form"):
    name = st.text_input("Name", placeholder="Drink Water")
    category = st.text_input("Category", placeholder="Health / Learning / Fitness")
    submitted = st.form_submit_button("Add Habit")
    if submitted:
        if not name.strip() or not category.strip():
            st.error("Please fill both Name and Category.")
        else:
            try:
                created = api_post("/api/habits", {"name": name.strip(), "category": category.strip()})
                st.success(f"Created habit #{created.get('id')}: {created.get('name')}")
                st.session_state.last_action = "created"
                time.sleep(0.6)
                st.experimental_rerun()
            except Exception as e:
                st.error(f"Failed to create habit: {e}")

# --- Filters ---
st.subheader("Your Habits")
col1, col2, col3 = st.columns([2,2,2])
with col1:
    sort_by = st.selectbox("Sort By", ["Newest", "Updated", "XP", "Streak"], index=0)
with col2:
    filt = st.text_input("Filter (name/category)", placeholder="Type to filter...")
with col3:
    st.write("")
    st.write("")
    refresh = st.button("🔄 Refresh")

# --- Data load ---
habits = []
load_error = None
try:
    habits = api_get("/api/habits")
except Exception as e:
    load_error = str(e)

if load_error:
    st.error(f"Could not load habits: {load_error}\n\nMake sure your API is running and CORS is enabled.")
else:
    # Filter
    if filt.strip():
        f = filt.lower()
        habits = [h for h in habits if (h.get('name','').lower().find(f) >= 0) or (h.get('category','').lower().find(f) >= 0)]

    # Sort
    if sort_by == "XP":
        habits.sort(key=lambda x: int(x.get("xp") or 0), reverse=True)
    elif sort_by == "Streak":
        habits.sort(key=lambda x: int(x.get("streak") or 0), reverse=True)
    elif sort_by == "Updated":
        habits.sort(key=lambda x: x.get("lastUpdated") or "", reverse=True)
    else:  # Newest ~ by id desc
        habits.sort(key=lambda x: int(x.get("id") or 0), reverse=True)

    # --- Render list ---
    if not habits:
        st.info("No habits yet. Add one above ✨")
    else:
        for h in habits:
            with st.container(border=True):
                top = st.columns([6,2,2,2])
                with top[0]:
                    st.markdown(f"**{h.get('name','')}**")
                    st.caption(h.get("category",""))
                with top[1]:
                    st.metric("XP", h.get("xp", 0))
                with top[2]:
                    st.metric("Streak", h.get("streak", 0))
                with top[3]:
                    st.caption("Updated")
                    st.write(human_time(str(h.get("lastUpdated",""))))

                # Actions
                a1, a2, a3, a4 = st.columns([2,2,2,6])
                if a1.button("Complete ✅", key=f"complete_{h['id']}"):
                    try:
                        updated = api_post(f"/api/habits/{h['id']}/complete")
                        st.success("Completed!")
                        st.session_state.last_action = "completed"
                        time.sleep(0.4)
                        st.experimental_rerun()
                    except Exception as e:
                        st.error(f"Complete failed: {e}")

                # Edit inline
                with a2.popover("Edit ✏️", use_container_width=True):
                    new_name = st.text_input("Name", value=h.get("name",""), key=f"edit_name_{h['id']}")
                    new_cat = st.text_input("Category", value=h.get("category",""), key=f"edit_cat_{h['id']}")
                    if st.button("Save", key=f"save_{h['id']}"):
                        try:
                            updated = api_put(f"/api/habits/{h['id']}", {"name": new_name, "category": new_cat})
                            st.success("Updated")
                            time.sleep(0.4)
                            st.experimental_rerun()
                        except Exception as e:
                            st.error(f"Update failed: {e}")

                if a3.button("Delete 🗑️", key=f"del_{h['id']}"):
                    try:
                        api_delete(f"/api/habits/{h['id']}")
                        st.warning("Deleted")
                        time.sleep(0.4)
                        st.experimental_rerun()
                    except Exception as e:
                        st.error(f"Delete failed: {e}")
