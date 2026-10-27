import { useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'

type ApiMessage = {
  tone: 'idle' | 'ok' | 'error'
  text: string
}

type UserProfile = {
  id: number
  displayName: string | null
  firstName: string | null
  lastName: string | null
  dateOfBirth: string | null
  postcode: string | null
}

type RunnerProfile = {
  id: number
  userProfileId: number
  bio: string | null
  experienceLevel: string | null
  minPaceSecondsPerKm: number | null
  maxPaceSecondsPerKm: number | null
  trainingFor: string | null
  preferredDistanceKm: number | null
  createdAt: string
  updatedAt: string
}

type UserProfileForm = {
  displayName: string
  firstName: string
  lastName: string
  dateOfBirth: string
  postcode: string
}

type RunnerProfileForm = {
  bio: string
  experienceLevel: string
  minPaceSecondsPerKm: string
  maxPaceSecondsPerKm: string
  trainingFor: string
  preferredDistanceKm: string
}

const emptyUserProfile: UserProfileForm = {
  displayName: '',
  firstName: '',
  lastName: '',
  dateOfBirth: '',
  postcode: '',
}

const emptyRunnerProfile: RunnerProfileForm = {
  bio: '',
  experienceLevel: '',
  minPaceSecondsPerKm: '',
  maxPaceSecondsPerKm: '',
  trainingFor: '',
  preferredDistanceKm: '',
}

async function readResponse(response: Response) {
  const contentType = response.headers.get('content-type') ?? ''

  if (contentType.includes('application/json')) {
    return response.json()
  }

  return response.text()
}

function optionalString(value: string) {
  const trimmed = value.trim()
  return trimmed.length > 0 ? trimmed : null
}

function optionalNumber(value: string) {
  if (value.trim().length === 0) return null
  return Number(value)
}

function App() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [message, setMessage] = useState<ApiMessage>({
    tone: 'idle',
    text: 'Register or log in to start testing your protected endpoints.',
  })
  const [lastResponse, setLastResponse] = useState<unknown>(null)
  const [userProfile, setUserProfile] =
    useState<UserProfileForm>(emptyUserProfile)
  const [runnerProfile, setRunnerProfile] =
    useState<RunnerProfileForm>(emptyRunnerProfile)

  async function callApi<T>(
    label: string,
    input: RequestInfo | URL,
    init?: RequestInit,
  ) {
    setMessage({ tone: 'idle', text: `${label}...` })

    const response = await fetch(input, {
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
        ...init?.headers,
      },
      ...init,
    })

    const body = await readResponse(response)
    setLastResponse(body || { status: response.status })

    if (!response.ok) {
      setMessage({
        tone: 'error',
        text: `${label} failed with ${response.status}.`,
      })
      return null
    }

    setMessage({ tone: 'ok', text: `${label} worked.` })
    return body as T
  }

  async function handleLogin(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    await callApi('Login', '/login?useCookies=true', {
      method: 'POST',
      body: JSON.stringify({ email, password }),
    })
  }

  async function handleSaveUserProfile(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const saved = await callApi<UserProfile>('Save user profile', '/api/user-profile/me', {
      method: 'PUT',
      body: JSON.stringify({
        displayName: optionalString(userProfile.displayName),
        firstName: optionalString(userProfile.firstName),
        lastName: optionalString(userProfile.lastName),
        dateOfBirth: userProfile.dateOfBirth || null,
        postcode: optionalString(userProfile.postcode),
      }),
    })

    if (saved) {
      setUserProfile({
        displayName: saved.displayName ?? '',
        firstName: saved.firstName ?? '',
        lastName: saved.lastName ?? '',
        dateOfBirth: saved.dateOfBirth?.slice(0, 10) ?? '',
        postcode: saved.postcode ?? '',
      })
    }
  }

  async function handleLoadUserProfile() {
    const loaded = await callApi<UserProfile>(
      'Load user profile',
      '/api/user-profile/me',
    )

    if (loaded) {
      setUserProfile({
        displayName: loaded.displayName ?? '',
        firstName: loaded.firstName ?? '',
        lastName: loaded.lastName ?? '',
        dateOfBirth: loaded.dateOfBirth?.slice(0, 10) ?? '',
        postcode: loaded.postcode ?? '',
      })
    }
  }

  async function handleSaveRunnerProfile(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const saved = await callApi<RunnerProfile>(
      'Save runner profile',
      '/api/runner-profile/me',
      {
        method: 'PUT',
        body: JSON.stringify({
          bio: optionalString(runnerProfile.bio),
          experienceLevel: optionalString(runnerProfile.experienceLevel),
          minPaceSecondsPerKm: optionalNumber(
            runnerProfile.minPaceSecondsPerKm,
          ),
          maxPaceSecondsPerKm: optionalNumber(
            runnerProfile.maxPaceSecondsPerKm,
          ),
          trainingFor: optionalString(runnerProfile.trainingFor),
          preferredDistanceKm: optionalNumber(
            runnerProfile.preferredDistanceKm,
          ),
        }),
      },
    )

    if (saved) {
      setRunnerProfile({
        bio: saved.bio ?? '',
        experienceLevel: saved.experienceLevel ?? '',
        minPaceSecondsPerKm: saved.minPaceSecondsPerKm?.toString() ?? '',
        maxPaceSecondsPerKm: saved.maxPaceSecondsPerKm?.toString() ?? '',
        trainingFor: saved.trainingFor ?? '',
        preferredDistanceKm: saved.preferredDistanceKm?.toString() ?? '',
      })
    }
  }

  async function handleLoadRunnerProfile() {
    const loaded = await callApi<RunnerProfile>(
      'Load runner profile',
      '/api/runner-profile/me',
    )

    if (loaded) {
      setRunnerProfile({
        bio: loaded.bio ?? '',
        experienceLevel: loaded.experienceLevel ?? '',
        minPaceSecondsPerKm: loaded.minPaceSecondsPerKm?.toString() ?? '',
        maxPaceSecondsPerKm: loaded.maxPaceSecondsPerKm?.toString() ?? '',
        trainingFor: loaded.trainingFor ?? '',
        preferredDistanceKm: loaded.preferredDistanceKm?.toString() ?? '',
      })
    }
  }

  return (
    <main className="app-shell">
      <header className="topbar">
        <div>
          <p className="eyebrow">RunnerMeets dev client</p>
          <h1>Auth and profile API flow</h1>
        </div>
        <div className={`status status-${message.tone}`}>{message.text}</div>
      </header>

      <section className="workspace">
        <form className="panel compact" onSubmit={handleLogin}>
          <div className="panel-heading">
            <h2>Account</h2>
            <p>POST /register then POST /login</p>
          </div>

          <label>
            Email
            <input
              autoComplete="email"
              onChange={(event) => setEmail(event.target.value)}
              placeholder="you@example.com"
              type="email"
              value={email}
            />
          </label>

          <label>
            Password
            <input
              autoComplete="current-password"
              onChange={(event) => setPassword(event.target.value)}
              placeholder="Password"
              type="password"
              value={password}
            />
          </label>

          <div className="button-row">
            <button
              className="secondary"
              onClick={(event) => {
                event.preventDefault()
                void callApi('Register', '/register', {
                  method: 'POST',
                  body: JSON.stringify({ email, password }),
                })
              }}
              type="button"
            >
              Register
            </button>
            <button type="submit">Login</button>
          </div>
        </form>

        <form className="panel" onSubmit={handleSaveUserProfile}>
          <div className="panel-heading">
            <h2>User profile</h2>
            <p>PUT /api/user-profile/me</p>
          </div>

          <div className="grid two">
            <label>
              Display name
              <input
                onChange={(event) =>
                  setUserProfile({
                    ...userProfile,
                    displayName: event.target.value,
                  })
                }
                value={userProfile.displayName}
              />
            </label>
            <label>
              Postcode
              <input
                onChange={(event) =>
                  setUserProfile({ ...userProfile, postcode: event.target.value })
                }
                value={userProfile.postcode}
              />
            </label>
            <label>
              First name
              <input
                onChange={(event) =>
                  setUserProfile({ ...userProfile, firstName: event.target.value })
                }
                value={userProfile.firstName}
              />
            </label>
            <label>
              Last name
              <input
                onChange={(event) =>
                  setUserProfile({ ...userProfile, lastName: event.target.value })
                }
                value={userProfile.lastName}
              />
            </label>
          </div>

          <label>
            Date of birth
            <input
              onChange={(event) =>
                setUserProfile({ ...userProfile, dateOfBirth: event.target.value })
              }
              type="date"
              value={userProfile.dateOfBirth}
            />
          </label>

          <div className="button-row">
            <button type="submit">Save profile</button>
            <button
              className="secondary"
              onClick={handleLoadUserProfile}
              type="button"
            >
              Load profile
            </button>
          </div>
        </form>

        <form className="panel" onSubmit={handleSaveRunnerProfile}>
          <div className="panel-heading">
            <h2>Runner profile</h2>
            <p>PUT /api/runner-profile/me</p>
          </div>

          <label>
            Bio
            <textarea
              onChange={(event) =>
                setRunnerProfile({ ...runnerProfile, bio: event.target.value })
              }
              rows={4}
              value={runnerProfile.bio}
            />
          </label>

          <div className="grid two">
            <label>
              Experience
              <input
                onChange={(event) =>
                  setRunnerProfile({
                    ...runnerProfile,
                    experienceLevel: event.target.value,
                  })
                }
                placeholder="beginner"
                value={runnerProfile.experienceLevel}
              />
            </label>
            <label>
              Training for
              <input
                onChange={(event) =>
                  setRunnerProfile({
                    ...runnerProfile,
                    trainingFor: event.target.value,
                  })
                }
                placeholder="10K"
                value={runnerProfile.trainingFor}
              />
            </label>
            <label>
              Min pace sec/km
              <input
                min="0"
                onChange={(event) =>
                  setRunnerProfile({
                    ...runnerProfile,
                    minPaceSecondsPerKm: event.target.value,
                  })
                }
                type="number"
                value={runnerProfile.minPaceSecondsPerKm}
              />
            </label>
            <label>
              Max pace sec/km
              <input
                min="0"
                onChange={(event) =>
                  setRunnerProfile({
                    ...runnerProfile,
                    maxPaceSecondsPerKm: event.target.value,
                  })
                }
                type="number"
                value={runnerProfile.maxPaceSecondsPerKm}
              />
            </label>
          </div>

          <label>
            Preferred distance km
            <input
              min="0"
              onChange={(event) =>
                setRunnerProfile({
                  ...runnerProfile,
                  preferredDistanceKm: event.target.value,
                })
              }
              step="0.1"
              type="number"
              value={runnerProfile.preferredDistanceKm}
            />
          </label>

          <div className="button-row">
            <button type="submit">Save runner profile</button>
            <button
              className="secondary"
              onClick={handleLoadRunnerProfile}
              type="button"
            >
              Load runner profile
            </button>
          </div>
        </form>

        <aside className="panel response-panel">
          <div className="panel-heading">
            <h2>Last response</h2>
            <p>Raw API output</p>
          </div>
          <pre>{JSON.stringify(lastResponse, null, 2)}</pre>
        </aside>
      </section>
    </main>
  )
}

export default App
